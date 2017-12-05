using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManageData
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; }
        public Table Table { get; set; }
        public Waiter Waiter { get; set; }
        public ICollection<DishInOrder> OrderedDishes { get; set; }
    }
}
