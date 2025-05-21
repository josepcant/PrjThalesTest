using PrjThalesTest.Data.Models;

namespace PrjThalesTest.Data.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllProductosAsync();
        Task<Producto> GetProductoByIdAsync(int id);
    }
}
