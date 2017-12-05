using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManagerData
{
    class UnitOfWork: IDisposable
    {
        Context _context = new Context();
        public OrdersRepository Orders { get; }
        public TablesRepository Tables { get; }

        public UnitOfWork()
        {
            Orders = new OrdersRepository(_context);
            Tables = new TablesRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
