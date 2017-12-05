using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManagerData
{
    class TablesRepository
    {
        private Context _context;

        public TablesRepository(Context context)
        {
            _context = context;
        }

        public void ChangeStatus()
        {
            // Vacant, Occupied, Reserved - changes on order completing
            //+ manually from vacant to reserved and vice versa
        }

        public void ChangeWaiter()
        {
            //Change assigned waiter
        }
    }
}
