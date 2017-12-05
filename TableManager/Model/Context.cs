using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManageData;

namespace TableManagerData
{
    class Context : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishInOrder> DishesInOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Waiter> Waiters { get; set; }

        //public Context() : base("DB") {

        //}
    }
}
