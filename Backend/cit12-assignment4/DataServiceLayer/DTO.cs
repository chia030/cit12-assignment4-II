namespace DataServiceLayer;

public class CategoryDTO
{
    public int Id { get; set; }
    public string? Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
}

public class ProductDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public CategoryDTO Category { get; set; } = null!;
}

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