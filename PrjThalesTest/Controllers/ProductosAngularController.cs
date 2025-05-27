using Microsoft.AspNetCore.Mvc;
using PrjThalesTest.Business.Interfaces;

namespace PrjThalesTest.Controllers
{
    public class ProductosAngularController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly ILogger<ProductosAngularController> _logger;

        public ProductosAngularController(IProductoService productoService, ILogger<ProductosAngularController> logger)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Vista principal con Angular
        public IActionResult Index()
        {
            return View();
        }

        // API endpoint para búsqueda de productos
        [HttpGet]
        public async Task<IActionResult> Search(int? id)
        {
            try
            {
                // Si no se proporciona ID, devolver todos los productos
                if (!id.HasValue)
                {
                    var todosProductos = await _productoService.GetAllProductosAsync();
                    return Json(new { success = true, data = todosProductos });
                }

                // Si se proporciona ID, buscar producto específico
                var producto = await _productoService.GetProductoByIdAsync(id.Value);
                if (producto == null)
                {
                    return Json(new { success = false, message = $"Producto con ID {id} no encontrado" });
                }

                // Devolver el producto como una lista para mantener consistencia
                return Json(new { success = true, data = new[] { producto } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar producto con ID: {id}");
                return Json(new { success = false, message = "Error al realizar la búsqueda" });
            }
        }

        // API endpoint para obtener detalles de un producto específico
        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            try
            {
                var producto = await _productoService.GetProductoByIdAsync(id);
                if (producto == null)
                {
                    return Json(new { success = false, message = $"Producto con ID {id} no encontrado" });
                }

                return Json(new { success = true, data = producto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles del producto con ID: {id}");
                return Json(new { success = false, message = "Error al cargar los detalles del producto" });
            }
        }
    }
}