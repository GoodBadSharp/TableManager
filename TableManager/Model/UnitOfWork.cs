using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManagerData
{
    public class UnitOfWork
    {
        Context _context = new Context();
        public IOrdersRepository Orders { get; }
        public ITablesRepository Tables { get; }

        public UnitOfWork()
        {
            Orders = new OrdersRepository(_context);
            Tables = new TablesRepository(_context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
