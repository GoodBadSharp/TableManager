using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManagerData.Model;

namespace TableManageData
{
    public class Table
    {
        public int Id { get; set; }

        [JsonProperty("Number_of_seats")]
        public int NumberOfSeats { get; set; }

        public string Location { get; set; }

        public TableStatus Status { get; set; }

        public ICollection<Order> RekatedOrders { get; set; }
    }
}
