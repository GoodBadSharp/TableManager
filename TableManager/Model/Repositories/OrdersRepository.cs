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
                order.Status = _context.OrderStatuses.Single(os => os.Id == 1);
                order.OrderTime = DateTime.Today;
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            catch
            { throw new InvalidOperationException("Failed to add the order"); }
        }

        public void UpdateOrder(Order order)
        {
            if (_context.Orders.Single(o => o.Id == order.Id) != null)
            {
                _context.Entry(order).State = EntityState.Modified;
                _context.SaveChanges();
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
                _context.Orders.Single(o => o.Id == orderId).Status.Id = 2;
                _context.SaveChanges();
            }
            catch { throw new InvalidOperationException("Failed to complete the order"); }
        }

        public void CancelOrder(int orderId)
        {
            try
            {
                _context.Orders.Remove(_context.Orders.First(o => o.Id == orderId));
                _context.SaveChanges();
            }
            catch { throw new InvalidOperationException("Failed to cancel the order"); }
        }

        public void GetDishInfo()
        {
            _context.SaveChanges();
        }
    }
}
