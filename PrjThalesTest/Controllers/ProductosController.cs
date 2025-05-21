using Microsoft.AspNetCore.Mvc;
using PrjThalesTest.Business.Interfaces;

namespace PrjThalesTest.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductoService productoService, ILogger<ProductosController> logger)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var productos = await _productoService.GetAllProductosAsync();
                return Json(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los productos");
                return StatusCode(500, "Error al cargar los productos");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var producto = await _productoService.GetProductoByIdAsync(id);

                if (producto == null)
                {
                    return NotFound($"Producto con ID {id} no encontrado");
                }

                return Json(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener producto con ID: {id}");
                return StatusCode(500, "Error al cargar el producto");
            }
        }
    }
}
