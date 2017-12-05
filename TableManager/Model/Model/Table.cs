using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManageData
{
    public class Table
    {
        public int Id { get; set; }
        public enum Status { Reserved, Occupied, Vacant }
        public int NumberOfSeats { get; set; }
        public string Location { get; set; }
        public ICollection<Order> CurrentOrders { get; set; }
    }
}
