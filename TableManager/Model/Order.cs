using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Order
    {
        public int Id { get; set; }
        public Table Table { get; set; }
        public List<Dish> Dishes { get; set; }
        public decimal Total(List<DishInOrder> dishes)
        {
            decimal total = 0;
            foreach (var dish in dishes)
            {
                total += dish.Dish.Price;
            }
            return total;
        }
        public Waiter Waiter { get; set; }

    }
}
