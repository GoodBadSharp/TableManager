namespace TableManagerData.Migrations
{
    using Newtonsoft.Json;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using TableManageData;
    using TableManagerData.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<TableManagerData.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TableManagerData.Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //debugger for packages console
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Launch();
            }

            Assembly assembly = Assembly.GetExecutingAssembly();

            GetDataFromJson<Dish>("TableManagerData.Model.SeedData.Dish.json", assembly, context, d => d.Id);
            GetDataFromJson<DishInOrder>("TableManagerData.Model.SeedData.DishInOrder.json", assembly, context, dio => new { dio.DishID, dio.OrderID });
            GetDataFromJson<Order>("TableManagerData.Model.SeedData.Order.json", assembly, context, o => o.Id);
            GetDataFromJson<OrderStatus>("TableManagerData.Model.SeedData.OrderStatus.json", assembly, context, os => os.Id);
            GetDataFromJson<Table>("TableManagerData.Model.SeedData.Table.json", assembly, context, t => t.Id);
            GetDataFromJson<TableStatus>("TableManagerData.Model.SeedData.TableStatus.json", assembly, context, ts => ts.Id);
            GetDataFromJson<Waiter>("TableManagerData.Model.SeedData.Waiter.json", assembly, context, w => w.Id);
        }

        private void GetDataFromJson<T>(string resourceName, Assembly assembly, Context context, Expression<Func<T, object>> KeyIdentifier) where T : class
        {
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        var dataArray = JsonConvert.DeserializeObject<T[]>(reader.ReadToEnd());
                        context.Set<T>().AddOrUpdate(KeyIdentifier, dataArray);
                    }
                }
            }
            catch
            {
                //    if (typeof(T) == typeof(Table))
                //        throw new InvalidOperationException("Failed to load table repository. Check if location property is unique");
                //   else
                throw new InvalidOperationException($"Failed to load {typeof(T).ToString()} repository");
            }
        }


    }
}
