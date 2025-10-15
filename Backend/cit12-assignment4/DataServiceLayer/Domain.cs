namespace DataServiceLayer;

public interface IEntity;

public interface IAggregateRoot : IEntity;

public interface IValueObject;

public interface IRepository;

// The test wants us to use bad practises
public class Order : IAggregateRoot
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime Required { get; set; }
    public DateTime? Shipped { get; set; }
    public double Freight { get; set; }
    public string? ShipName { get; set; }
    public string? ShipCity { get; set; }

    public ICollection<OrderDetails>? OrderDetails { get; set; } = null;
}

// The test wants us to use bad practises
public class OrderDetails : IEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public double UnitPrice { get; set; }
    public double Quantity { get; set; }
    public double Discount { get; set; }
}

public class Product : IAggregateRoot
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double UnitPrice { get; set; }
    public string? QuantityPerUnit { get; set; }
    public int UnitsInStock { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
}

public class Category : IAggregateRoot
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}