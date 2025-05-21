using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PrjThalesTest.Data.Interfaces;
using PrjThalesTest.Data.Models;

namespace PrjThalesTest.Data.Repositories
{
    internal class ProductoRepository : IProductoRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductoRepository> _logger;
        private const string BaseUrl = "https://api.escuelajs.co/api/v1";

        public ProductoRepository(HttpClient httpClient, ILogger<ProductoRepository> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los productos");
                return await _httpClient.GetFromJsonAsync<List<Producto>>($"{BaseUrl}/products");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los productos");
                throw;
            }
        }

        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Obteniendo producto con ID: {id}");
                return await _httpClient.GetFromJsonAsync<Producto>($"{BaseUrl}/products/{id}");
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                _logger.LogWarning($"Producto con ID {id} no encontrado");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener producto con ID: {id}");
                throw;
            }
        }
    }
}
