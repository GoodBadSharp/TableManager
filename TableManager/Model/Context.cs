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
        //Direct access to theses sets is unecessary. They can still be indirectly accessed via Orders set
        //public DbSet<Dish> Dishes { get; set; }
        //public DbSet<DishInOrder> DishesInOrders { get; set; }
        //public DbSet<Table> Tables { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Waiter> Waiters { get; set; }

        //public Context() : base("DB") {

        //}

        //Move to Configuration Seed method
        //private void GetDataFromJson<T>(string resourceName, Assembly assembly, DataContext context, Expression<Func<T, object>> KeyIdentifier) where T : class
        //{
        //    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        //    {
        //        using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
        //        {
        //            var dataArray = JsonConvert.DeserializeObject<T[]>(reader.ReadToEnd());
        //            context.Set<T>().AddOrUpdate(KeyIdentifier, dataArray);
        //        }
        //    }
        //}
    }


}
