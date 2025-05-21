using Microsoft.AspNetCore.Mvc;
using PrjThalesTest.Business.Interfaces;
using PrjThalesTest.Business.ModelsDto;

namespace PrjThalesTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductoService productoService, ILogger<ProductosController> logger)
        {
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los productos con sus impuestos calculados
        /// </summary>
        /// <returns>Lista de productos con impuestos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetAllProductos()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los productos desde la API");
                var productos = await _productoService.GetAllProductosAsync();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los productos");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un producto específico por su ID con el impuesto calculado
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto con impuesto</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoDTO>> GetProductoById(int id)
        {
            try
            {
                _logger.LogInformation($"Obteniendo producto con ID: {id} desde la API");
                var producto = await _productoService.GetProductoByIdAsync(id);

                if (producto == null)
                {
                    return NotFound($"Producto con ID {id} no encontrado");
                }

                return Ok(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener producto con ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
