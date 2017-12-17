using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManagerData.Interfaces;

namespace TableManagerData
{
    public sealed class UnitOfWork : IDisposable
    {
        Context _context = new Context();

        //private static readonly Lazy<UnitOfWork> _lazyInstance = new Lazy<UnitOfWork>(() => new UnitOfWork());

        //public static UnitOfWork Instance { get { return _lazyInstance.Value; } }

        public IOrdersRepository Orders { get; }

        public ITablesRepository Tables { get; }

        public IQueries Queries { get; }

        public UnitOfWork()
        {
            Orders = new OrdersRepository(_context);
            Tables = new TablesRepository(_context);
            Queries = new Queries(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
