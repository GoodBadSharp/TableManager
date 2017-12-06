using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManageData;

namespace TableManagerData.Model
{
    public class TableStatus
    {
        public int Id { get; set; }

        public string Description { get; set; } //Reserved, Occupied, Vacant 

        public ICollection<Table> Tables { get; set; }
    }
}
