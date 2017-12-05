using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManageData
{
    public class Waiter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> CurrentOrders { get; set; }
    }
}
