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

        //public Context() : base("DB") {

        //}

        //Move the to Configuration method
        //private void GetDataFromJson<T>(string resourceName, Assembly assembly, DataContext context, Expression<Func<T, object>> KeyIdentifier) where T : class
        //{
        //    try
        //    {
        //        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        //        {
        //            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
        //            {
        //                var dataArray = JsonConvert.DeserializeObject<T[]>(reader.ReadToEnd());
        //                context.Set<T>().AddOrUpdate(KeyIdentifier, dataArray);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        if (typeof(T) == typeof(Table))
        //            throw new InvalidOperationException("Failed to load table repository. Check if location property is unique");
        //        else
        //            throw new InvalidOperationException($"Failed to load {typeof(T).ToString()} repository");
        //    }
        //}
    }


}
