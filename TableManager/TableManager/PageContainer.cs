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
        private static TablesPage _tablesPage = new TablesPage();
        private static AddOrderPage _addOrderPage = new AddOrderPage();
        private static EditOrderPage _editOrderPage = new EditOrderPage();       
        private static StatisticsPage _statsPage = new StatisticsPage();

        public static EditOrderPage EditOrderPage { get { return _editOrderPage; } }
        public static AddOrderPage AddOrderPage { get { return _addOrderPage; } }
        public static TablesPage TablesPage { get { return _tablesPage; } }
        public static StatisticsPage StatsPage { get { return _statsPage; } }

        /// <summary>
        /// Creates DishInOrder instance for Order object 
        /// </summary>
        /// <param name="dishId">Id of the dish to be added</param>
        /// <param name="quantity">Quantity of the dish to added</param>
        /// <returns>instance of DishInOrder</returns>
        public static DishInOrder AddDish(int dishId, int quantity)
        {
            DishInOrder dishInOrder = new DishInOrder();
            dishInOrder.DishID = dishId;
            dishInOrder.Quantity = quantity;
            return dishInOrder;
        }
    }
}
