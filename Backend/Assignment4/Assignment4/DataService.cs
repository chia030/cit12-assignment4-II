using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
    public class DataService
    {

        //SOME DTO's
        
        public List<Category> GetCategories()
        {
            //SOME SQL
            
            //MAP THE DATA TO THE CLASS
        }
        
        
        [Fact]
        public void GetAllCategories_NoArgument_ReturnsAllCategories()
        {
            var service = new DataService();
            var categories = service.GetCategories();
            Assert.Equal(8, categories.Count);
            Assert.Equal("Beverages", categories.First().Name);
        }
    }

    public interface IEntity;

    public interface IAggregateRoot : IEntity;

    public interface IValueObject;

    public interface IRepository;

    // The test wants us to use bad practises
    public class Order : IAggregateRoot
    {
        public int Id { get; set; }
        public DateTime Date {get; set;}
        public DateTime Required { get; set; }
        public bool Shipped { get; set; }
        public OrderDetails OrderDetails { get; set; }
        public string ShipName { get; set; }
        public string ShipCity { get; set; }
    }

    // The test wants us to use bad practises
    public class OrderDetails : IEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public float UnitPrice { get; set; }
        public float Quantity { get; set; }
        public float Discount { get; set; }
    }

    public class Product : IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float UnitPrice { get; set; }
        public float QuantityPerUnit { get; set; }
        public int UnitsInStock { get; set; }
    }

    public class Category : IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
