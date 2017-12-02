using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Table
    {
        public int Id { get; set; }
        public enum State
        {
            Reserved, Occupied, Vacant
        }
        public List<Order> CurrentOrders { get; set; }
    }
}
