using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManagerData.Interfaces;
using TableManagerData.QueryLogic;

namespace TableManagerData
{
    public class Queries : IQueries
    {
        public event Action<IEnumerable<QueryContainer>, int, string> QueryCollectionHandler;

        Context _context;

        public Queries(Context context)
        {
            _context = context;
        }

        public void ConductQuery(int queryID)
        {
            switch (queryID)
            {
                case 1:
                    //UpdateColumnsHandler?.Invoke(new List<string> { "ID", "Terminal's Location", "Products out of stock" }, new List<string> { "TerminalID", "Location", "ReportDetails" });
                    TableStats();
                    break;
                case 2:
                    //UpdateColumnsHandler?.Invoke(new List<string> { "ID", "Product Name", "Sold" }, new List<string> { "ProductID", "Name", "ReportDetails" });
                    StaffStats();
                    break;
                default:
                    throw new NotImplementedException("Query not implemented");
            }
        }

        public void GetQueryInfo()
        {
            var tableStatsQuery = new QueryContainer(1, "Table Statistics");
            throw new NotImplementedException();
        }

        void TableStats()
        {

        }

        void StaffStats()
        {

        }
    }
}
