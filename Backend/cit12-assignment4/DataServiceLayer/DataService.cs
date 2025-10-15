using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer;

public class DataService
{
    private readonly NorthwindContext _context;

    public DataService()
    {
        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=northwind;Username=postgres;Password=1234")
            .Options;

        _context = new NorthwindContext(options);
    }

    /* ------------------------- UTIL ------------------------- */

    private int GetNextId<TEntity>(DbSet<TEntity> dbSet, Func<TEntity, int> idSelector) where TEntity : class
    {
        return dbSet.Any()
            ? dbSet.Max(idSelector) + 1
            : 1;
    }

    /* ------------------------- CATEGORY ------------------------- */

    public List<Category> GetCategories()
    {
        return _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToList();
    }

    public Category? GetCategory(int id)
    {
        return _context.Categories
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == id);
    }

    public Category CreateCategory(string name, string description)
    {
        var category = new Category
        {
            Id = GetNextId(_context.Categories, c => c.Id),
            Name = name,
            Description = description
        };

        _context.Categories.Add(category);
        _context.SaveChanges();
        return category;
    }

    public bool DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
            return false;

        _context.Categories.Remove(category);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateCategory(int id, string newName, string newDescription)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
            return false;

        category.Name = newName;
        category.Description = newDescription;
        _context.SaveChanges();
        return true;
    }

    /* ------------------------- PRODUCTS ------------------------- */

    public Product? GetProduct(int id)
    {
        return _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);
    }

    public List<ProductByCategoryDto> GetProductByCategory(int categoryId)
    {
        return _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Id)
            .Select(p => new ProductByCategoryDto
            {
                Id = p.Id,
                Name = p.Name!,
                CategoryName = p.Category!.Name!
            })
            .ToList();
    }

    public List<ProductByNameDto> GetProductByName(string substring)
    {
        substring = substring.ToLower();

        return _context.Products
            .AsNoTracking()
            .Where(p => p.Name!.ToLower().Contains(substring))
            .OrderBy(p => p.Id)
            .Select(p => new ProductByNameDto
            {
                ProductId = p.Id,
                ProductName = p.Name!
            })
            .ToList();
    }

    public Product CreateProduct(string name, float unitPrice, string? quantityPerUnit, int unitsInStock, int categoryId)
    {
        var product = new Product
        {
            Id = GetNextId(_context.Products, p => p.Id),
            Name = name,
            UnitPrice = unitPrice,
            QuantityPerUnit = quantityPerUnit,
            UnitsInStock = unitsInStock,
            CategoryId = categoryId
        };

        _context.Products.Add(product);
        _context.SaveChanges();
        return product;
    }

    /* ------------------------- ORDERS ------------------------- */

    public Order? GetOrder(int id)
    {
        return _context.Orders
            .Include(o => o.OrderDetails!)
                .ThenInclude(od => od.Product)
                    .ThenInclude(p => p.Category)
            .AsNoTracking()
            .FirstOrDefault(o => o.Id == id);
    }

    public List<Order> GetOrders()
    {
        return _context.Orders
            .AsNoTracking()
            .OrderBy(o => o.Id)
            .ToList();
    }

    public Order CreateOrder(DateTime orderDate, DateTime requiredDate, DateTime shippedDate, float freight, string shipName, string shipCity)
    {
        var order = new Order
        {
            Id = GetNextId(_context.Orders, o => o.Id),
            Date = orderDate,
            Required = requiredDate,
            Shipped = shippedDate,
            Freight = freight,
            ShipName = shipName,
            ShipCity = shipCity
        };

        _context.Orders.Add(order);
        _context.SaveChanges();
        return order;
    }

    /* ------------------------- ORDER DETAILS ------------------------- */

    public List<OrderDetails> GetOrderDetailsByOrderId(int orderId)
    {
        return _context.OrderDetails
            .Include(od => od.Product)
            .Include(od => od.Order)
            .AsNoTracking()
            .Where(od => od.OrderId == orderId)
            .OrderBy(od => od.ProductId)
            .ToList();
    }

    public List<OrderDetails> GetOrderDetailsByProductId(int productId)
    {
        return _context.OrderDetails
            .Include(od => od.Product)
            .Include(od => od.Order)
            .AsNoTracking()
            .Where(od => od.ProductId == productId)
            .OrderBy(od => od.OrderId)
            .ToList();
    }

    public OrderDetails CreateOrderDetails(int orderId, int productId, float unitPrice, short quantity, float discount)
    {
        var orderDetails = new OrderDetails
        {
            OrderId = orderId,
            ProductId = productId,
            UnitPrice = unitPrice,
            Quantity = quantity,
            Discount = discount
        };

        _context.OrderDetails.Add(orderDetails);
        _context.SaveChanges();
        return orderDetails;
    }
}