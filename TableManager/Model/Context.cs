using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManageData;
using TableManagerData.Model;

namespace TableManagerData
{
    public class Context : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishInOrder> DishesInOrders { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Waiter> Waiters { get; set; }

        public DbSet<TableStatus> TableStatuses { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public Context() : base("localsql") {

        }
    }
}
