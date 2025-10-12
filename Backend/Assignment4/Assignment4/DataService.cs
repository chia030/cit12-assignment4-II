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
    }

    public interface IEntity;

    public interface IAggregateRoot : IEntity;

    public interface IValueObject;

    public interface IRepository;

    public class Order : IAggregateRoot
    {
        private Guid ID;
        private DateOnly Date;
        

    }

    public class Product : IAggregateRoot
    {

    }
}
