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
        public event Action<int> UpdateTableByIdHandler;

        private Context _context;

        public OrdersRepository(Context context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            //try
            //{
                order.Status_Id = 1;
                //_context.SaveChanges();
                _context.Orders.Add(order);
                Table relatedTable = _context.Tables.Single(t => t.Id == order.Table_Id);
                relatedTable.Status_Id = 2;
                _context.SaveChanges();
                UpdateTableByIdHandler?.Invoke(relatedTable.Id);
            //}
            //catch
            //{ throw new InvalidOperationException("Failed to add the order"); }
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
            //try
            //{
                Order completedOrder = _context.Orders.Single(o => o.Id == orderId);
                completedOrder.Status_Id = 2;
                Table relatedTable = _context.Tables.Single(t => t.Id == completedOrder.Table_Id);
                if (relatedTable.RelatedOrders.FirstOrDefault(o => o.Status_Id == 1) == null)
                    _context.Tables.Single(t => t.Id == completedOrder.Table_Id).Status_Id = 1;

                _context.SaveChanges();
                UpdateTableByIdHandler?.Invoke(relatedTable.Id);
            //}
            //catch { throw new InvalidOperationException("Failed to complete the order"); }
        }

        public void CancelOrder(int orderId)
        {
            try
            {
                Order canceledOrder = _context.Orders.Single(o => o.Id == orderId);
                _context.Orders.Remove(canceledOrder);
                Table relatedTable = _context.Tables.Single(t => t.Id == canceledOrder.Table_Id);
                if (relatedTable.RelatedOrders.FirstOrDefault(o => o.Status_Id == 1) == null)
                    _context.Tables.Single(t => t.Id == canceledOrder.Table_Id).Status_Id = 1;

                _context.SaveChanges();
                UpdateTableByIdHandler?.Invoke(relatedTable.Id);
            }
            catch { throw new InvalidOperationException("Failed to cancel the order. Refresh tables page"); }
        }

        public void GetDishInfo()
        {
            _context.SaveChanges();
        }
    }
}
