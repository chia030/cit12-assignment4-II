using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer;

public class NorthwindContext(DbContextOptions<NorthwindContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetails> OrderDetails => Set<OrderDetails>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure if not configured externally (for example, in Program.cs)
        // if (optionsBuilder.IsConfigured) return;
        const string connectionString =
            "Host=localhost;Port=5432;Database=northwind;Username=postgres;Password=amogus";
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
                .ValueGeneratedOnAdd(); // PostgreSQL generates ID

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
                .ValueGeneratedOnAdd(); // generated ID

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
                .ValueGeneratedOnAdd(); // generated ID

            entity.Property(e => e.Date)
                .HasColumnName("orderdate");

            entity.Property(e => e.Required) // property name must match your class
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