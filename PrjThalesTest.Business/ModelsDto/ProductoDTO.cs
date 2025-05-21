using PrjThalesTest.Data.Models;

namespace PrjThalesTest.Business.ModelsDto
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public decimal Impuesto { get; set; }
        public decimal PrecioConImpuesto { get; set; }
        public string Description { get; set; }
        public Categoria Category { get; set; }
        public List<string> Images { get; set; }
        public DateTime CreationAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static ProductoDTO FromProducto(Producto producto, decimal impuestoRate = 0.19m)
        {
            if (producto == null) return null;

            var impuesto = producto.Price * impuestoRate;

            return new ProductoDTO
            {
                Id = producto.Id,
                Title = producto.Title,
                Slug = producto.Slug,
                Price = producto.Price,
                Impuesto = impuesto,
                PrecioConImpuesto = producto.Price + impuesto,
                Description = producto.Description,
                Category = producto.Category,
                Images = producto.Images,
                CreationAt = producto.CreationAt,
                UpdatedAt = producto.UpdatedAt
            };
        }
    }
}
