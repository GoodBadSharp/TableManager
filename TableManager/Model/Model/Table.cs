using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        [Index("Location_Index", IsUnique = true)]
        public string Location { get; set; }

        [ForeignKey("TableStatus")]
        public int StatusID { get; set; }

        public TableStatus Status { get; set; }

        public ICollection<Order> RelatedOrders { get; set; }
    }
}
