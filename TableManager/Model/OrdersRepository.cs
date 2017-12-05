using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManageData;

namespace TableManagerData
{
    class OrdersRepository
    {
        private Context _context;

        public OrdersRepository(Context context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            //Logic for adding new order
        }

        public void ModifyOrder(Order order)
        {
            //Logic for modifying active order
        }

        public void OrderComplete()
        {
            //Change order status to complete
            //Write information to BD
        }
    }
}
