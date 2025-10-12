#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Data.Domain;

using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
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
                .Include(o => o.OrderDetails)
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

    /* ------------------------- DTOs ------------------------- */

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
}


namespace Northwind.Data
{
    public class NorthwindContext(DbContextOptions<NorthwindContext> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetails> OrderDetails => Set<OrderDetails>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure if not configured externally (for example, in Program.cs)
            if (optionsBuilder.IsConfigured) return;
            const string connectionString =
                "Host=localhost;Port=5432;Database=northwind;Username=postgres;Password=1234";
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* ------------------------- CATEGORY ------------------------- */
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("categoryid")
                    .ValueGeneratedOnAdd(); // ✅ PostgreSQL generates ID

                entity.Property(e => e.Name)
                    .HasColumnName("categoryname");

                entity.Property(e => e.Description)
                    .HasColumnName("description");
            });

            /* ------------------------- PRODUCT ------------------------- */
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("productid")
                    .ValueGeneratedOnAdd(); // ✅ generated ID

                entity.Property(e => e.Name)
                    .HasColumnName("productname");

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("unitprice");

                entity.Property(e => e.QuantityPerUnit)
                    .HasColumnName("quantityperunit");

                entity.Property(e => e.UnitsInStock)
                    .HasColumnName("unitsinstock");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryid");

                // Relationship: Product → Category (many-to-one)
                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId);
            });

            /* ------------------------- ORDER ------------------------- */
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("orderid")
                    .ValueGeneratedOnAdd(); // ✅ generated ID

                entity.Property(e => e.Date)
                    .HasColumnName("orderdate");

                entity.Property(e => e.Required) // ✅ property name must match your class
                    .HasColumnName("requireddate");

                entity.Property(e => e.Shipped)
                    .HasColumnName("shippeddate");

                entity.Property(e => e.Freight)
                    .HasColumnName("freight");

                entity.Property(e => e.ShipName)
                    .HasColumnName("shipname");

                entity.Property(e => e.ShipCity)
                    .HasColumnName("shipcity");
            });

            /* ------------------------- ORDER DETAILS ------------------------- */
            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.ToTable("orderdetails");

                // Composite Key
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderid");

                entity.Property(e => e.ProductId)
                    .HasColumnName("productid");

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("unitprice");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity");

                entity.Property(e => e.Discount)
                    .HasColumnName("discount");

                // Relationships
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(e => e.OrderId);

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(e => e.ProductId);
            });
        }

    }

    namespace Domain
    {
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
    }
}