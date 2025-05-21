using PrjThalesTest.Business.ModelsDto;

namespace PrjThalesTest.Business.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDTO>> GetAllProductosAsync();
        Task<ProductoDTO> GetProductoByIdAsync(int id);
        decimal CalcularImpuesto(decimal precio);
    }
}
