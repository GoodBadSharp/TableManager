﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using TableManagerData.Model;

namespace TableManageData
{
    public class Order
    {
        public int Id { get; set; }

        [JsonProperty("Time")]
        public DateTime OrderTime { get; set; }

        public OrderStatus Status { get; set; }

        public Table Table { get; set; }

        public Waiter Waiter { get; set; }

        public ICollection<DishInOrder> OrderedDishes { get; set; }
    }
}
