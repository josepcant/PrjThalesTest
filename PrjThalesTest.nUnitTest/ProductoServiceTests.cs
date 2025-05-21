using Microsoft.Extensions.Logging;
using Moq;
using PrjThalesTest.Business.Interfaces;
using PrjThalesTest.Business.ModelsDto;
using PrjThalesTest.Business.Services;
using PrjThalesTest.Data.Interfaces;
using PrjThalesTest.Data.Models;

namespace PrjThalesTest.nUnitTest
{
    public class ProductoServiceTests
    {
        private readonly Mock<IProductoRepository> _mockRepository;
        private readonly Mock<ILogger<ProductoService>> _mockLogger;
        private readonly IProductoService _productoService;

        public ProductoServiceTests()
        {
            _mockRepository = new Mock<IProductoRepository>();
            _mockLogger = new Mock<ILogger<ProductoService>>();
            _productoService = new ProductoService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void CalcularImpuesto_PrecioPositivo_RetornaImpuestoCalculado()
        {
            // Arrange
            decimal precio = 100m;
            decimal expectedImpuesto = 19m; // 19% de 100

            // Act
            decimal resultado = _productoService.CalcularImpuesto(precio);

            // Assert
            Assert.Equal(expectedImpuesto, resultado);
        }

        [Fact]
        public void CalcularImpuesto_PrecioCero_RetornaCero()
        {
            // Arrange
            decimal precio = 0m;
            decimal expectedImpuesto = 0m;

            // Act
            decimal resultado = _productoService.CalcularImpuesto(precio);

            // Assert
            Assert.Equal(expectedImpuesto, resultado);
        }

        [Fact]
        public void CalcularImpuesto_PrecioNegativo_LanzaArgumentException()
        {
            // Arrange
            decimal precio = -100m;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _productoService.CalcularImpuesto(precio));
            Assert.Contains("no puede ser negativo", exception.Message);
        }

        [Fact]
        public async Task GetProductoByIdAsync_ProductoExiste_RetornaProductoConImpuestoCalculado()
        {
            // Arrange
            int id = 1;
            decimal precio = 100m;
            decimal expectedImpuesto = 19m; // 19% de 100

            var mockProducto = new Producto
            {
                Id = id,
                Title = "Test Product",
                Price = precio,
                Description = "Test Description",
                Category = new Categoria { Id = 1, Name = "Test Category" },
                Images = new List<string> { "test-image.jpg" },
                CreationAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _mockRepository.Setup(repo => repo.GetProductoByIdAsync(id))
                .ReturnsAsync(mockProducto);

            // Act
            var result = await _productoService.GetProductoByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(precio, result.Price);
            Assert.Equal(expectedImpuesto, result.Impuesto);
            Assert.Equal(precio + expectedImpuesto, result.PrecioConImpuesto);
        }

        [Fact]
        public async Task GetProductoByIdAsync_ProductoNoExiste_RetornaNull()
        {
            // Arrange
            int id = 999;
            _mockRepository.Setup(repo => repo.GetProductoByIdAsync(id))
                .ReturnsAsync((Producto)null);

            // Act
            var result = await _productoService.GetProductoByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllProductosAsync_ExistenProductos_RetornaListaConImpuestosCalculados()
        {
            // Arrange
            var mockProductos = new List<Producto>
            {
                new Producto
                {
                    Id = 1,
                    Title = "Product 1",
                    Price = 100m,
                    Description = "Description 1",
                    Category = new Categoria { Id = 1, Name = "Category 1" },
                    Images = new List<string> { "image1.jpg" },
                    CreationAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Producto
                {
                    Id = 2,
                    Title = "Product 2",
                    Price = 200m,
                    Description = "Description 2",
                    Category = new Categoria { Id = 2, Name = "Category 2" },
                    Images = new List<string> { "image2.jpg" },
                    CreationAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            _mockRepository.Setup(repo => repo.GetAllProductosAsync())
                .ReturnsAsync(mockProductos);

            // Act
            var result = await _productoService.GetAllProductosAsync();
            var resultList = new List<ProductoDTO>(result);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, resultList.Count);

            // Verificar cálculos para el primer producto
            Assert.Equal(100m, resultList[0].Price);
            Assert.Equal(19m, resultList[0].Impuesto); // 19% de 100
            Assert.Equal(119m, resultList[0].PrecioConImpuesto);

            // Verificar cálculos para el segundo producto
            Assert.Equal(200m, resultList[1].Price);
            Assert.Equal(38m, resultList[1].Impuesto); // 19% de 200
            Assert.Equal(238m, resultList[1].PrecioConImpuesto);
        }
    }
}
