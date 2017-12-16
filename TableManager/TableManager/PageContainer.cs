using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManageData;
using TableManager;

namespace TableManager
{
    static class PageContainer
    {
        private static AddOrderPage _addOrderPage = new AddOrderPage();
        private static EditOrderPage _editOrderPage = new EditOrderPage();
        private static TablesPage _tablesPage = new TablesPage();
        private static StatisticsPage _statsPage = new StatisticsPage();

        public static AddOrderPage AddOrderPage { get { return _addOrderPage; } }
        public static EditOrderPage EditOrderPage { get { return _editOrderPage; } }
        public static TablesPage TablesPage { get { return _tablesPage; } }
        public static StatisticsPage StatsPage { get { return _statsPage; } }

        public static DishInOrder AddDish(int dishId, int quantity)
        {
            DishInOrder dish = new DishInOrder();
            dish.DishID = dishId;
            dish.Quantity = quantity;
            return dish;
        }
    }
}
