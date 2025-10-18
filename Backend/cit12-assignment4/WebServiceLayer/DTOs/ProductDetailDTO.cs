namespace WebServiceLayer.DTOs
{
    public class ProductDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public CategoryDTO Category { get; set; } = null!;
    }
}
