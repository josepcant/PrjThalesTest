using Microsoft.Extensions.Logging;
using PrjThalesTest.Business.Interfaces;
using PrjThalesTest.Business.ModelsDto;
using PrjThalesTest.Data.Interfaces;

namespace PrjThalesTest.Business.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ILogger<ProductoService> _logger;
        private const decimal _impuestoRate = 0.19m; // 19%

        public ProductoService(IProductoRepository productoRepository, ILogger<ProductoService> logger)
        {
            _productoRepository = productoRepository ?? throw new ArgumentNullException(nameof(productoRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllProductosAsync()
        {
            var productos = await _productoRepository.GetAllProductosAsync();
            return productos.Select(p => ProductoDTO.FromProducto(p, _impuestoRate));
        }

        public async Task<ProductoDTO> GetProductoByIdAsync(int id)
        {
            var producto = await _productoRepository.GetProductoByIdAsync(id);
            return ProductoDTO.FromProducto(producto, _impuestoRate);
        }

        public decimal CalcularImpuesto(decimal precio)
        {
            if (precio < 0)
            {
                _logger.LogWarning($"Se intentó calcular impuesto para un precio negativo: {precio}");
                throw new ArgumentException("El precio no puede ser negativo", nameof(precio));
            }

            return Math.Round(precio * _impuestoRate, 2);
        }
    }
}
