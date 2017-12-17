using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManageData;

namespace TableManagerData
{
    internal class OrdersRepository : IOrdersRepository
    {
        private Context _context;

        public OrdersRepository(Context context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            try
            {
                order.Status_Id = 1;
                Table relatedTable = _context.Tables.Single(t => t.Id == order.Table_Id);
                relatedTable.Status_Id = 2;
                _context.Entry(order).State = EntityState.Added;
            }
            catch
            { throw new InvalidOperationException("Failed to add the order"); }
        }

        public void UpdateOrder(Order order)
        {
            if (_context.Orders.Single(o => o.Id == order.Id) != null)
            {
                _context.Entry(order).State = EntityState.Modified;
            }
            else
                throw new InvalidOperationException("Cannot update the order because it wasn't found in the database. Refresh application");
        }

        public IEnumerable<Dish> GetDishes()
        {
            return _context.Dishes.AsNoTracking<Dish>();
        }

        public IEnumerable<Order> GetActiveOrders(int tablesId)
        {
            return _context.Orders.Include(o => o.OrderedDishes.Select(dio => dio.Dish))
                        .Where(o => o.Table_Id == tablesId)
                        .Where(o => o.Status_Id == 1)
                        .AsNoTracking();
        }

        public void OrderComplete(int orderId)
        {
            try
            {
                Order completedOrder = _context.Orders.Single(o => o.Id == orderId);
                Table relatedTable = _context.Tables.Single(t => t.Id == completedOrder.Table_Id);
                completedOrder.Status_Id = 2;
                _context.Entry(completedOrder).State = EntityState.Modified;
                _context.SaveChanges();
                if (_context.Orders.Where(o => o.Table_Id == relatedTable.Id && o.Status_Id == 1).Count() == 0)
                    relatedTable.Status_Id = 1;
            }
            catch { throw new InvalidOperationException("Failed to complete the order"); }
        }

        public void CancelOrder(int orderId)
        {
            try
            {
                Order canceledOrder = _context.Orders.Single(o => o.Id == orderId);
                Table relatedTable = _context.Tables.Single(t => t.Id == canceledOrder.Table_Id);
                _context.Entry(canceledOrder).State = EntityState.Deleted;
                _context.SaveChanges();
                if (_context.Orders.Where(o => o.Table_Id == relatedTable.Id && o.Status_Id == 1).Count() == 0)
                    relatedTable.Status_Id = 1;
            }
            catch { throw new InvalidOperationException("Failed to cancel the order. Refresh tables page"); }
        }

    }
}
