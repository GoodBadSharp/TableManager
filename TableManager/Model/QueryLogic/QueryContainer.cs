using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManagerData.QueryLogic
{
    public class QueryContainer
    {
        public int QueryID { get; set; }

        public string Description { get; set; }

        public QueryContainer(int id, string desc)
        {
            QueryID = id;
            Description = desc;
        }
    }
}
