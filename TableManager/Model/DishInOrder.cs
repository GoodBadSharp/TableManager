using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class DishInOrder
    {
        public int Id { get; set; }
        public Dish Dish { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
