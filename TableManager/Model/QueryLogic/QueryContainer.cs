using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManagerData.QueryLogic
{
    public class QueryContainer
    {
        public int ReportID { get; set; }
        public string Description { get; set; }

        public QueryContainer(int id, string desc)
        {
            ReportID = id;
            Description = desc;
        }
    }
}
