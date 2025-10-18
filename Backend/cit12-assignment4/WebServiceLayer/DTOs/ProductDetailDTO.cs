namespace WebServiceLayer.DTOs
{
    public class ProductDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double? UnitPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public CategoryDTO Category { get; set; } = null!;
    }
}
