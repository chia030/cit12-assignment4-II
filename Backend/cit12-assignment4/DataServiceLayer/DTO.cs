namespace DataServiceLayer;

public class ProductByCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
}

public class ProductByNameDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
}