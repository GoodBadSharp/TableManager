using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManagerData.Model;

namespace TableManageData
{
    public class Table
    {
        public int Id { get; set; }

        public int NumberOfSeats { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        [ForeignKey("Status"), Required]
        public int Status_Id { get; set; }

        public TableStatus Status { get; set; }

        public ICollection<Order> RelatedOrders { get; set; }
    }
}
