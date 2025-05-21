namespace PrjThalesTest.Data.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Categoria Category { get; set; }
        public List<string> Images { get; set; }
        public DateTime CreationAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
