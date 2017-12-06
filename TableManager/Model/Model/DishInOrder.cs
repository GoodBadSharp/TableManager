using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManageData
{
    public class DishInOrder
    {
        [Key, Column(Order = 0), ForeignKey("Dish"), JsonProperty("Dish_Id")]
        public int DishID { get; set; }

        [Key, Column(Order = 1), ForeignKey("Order"), JsonProperty("Order_id")]
        public int OrderID { get; set; }

        public int Quantity { get; set; }

        public Dish Dish { get; set; }

        public Order Order { get; set; }
    }
}
