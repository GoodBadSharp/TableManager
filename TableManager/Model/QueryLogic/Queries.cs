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
        public event Action<IEnumerable<QueryContainer>, string, string> QueryCollectionHandler;
        public event Func<DateTime> GetSpecifiedFromDateCallback;
        public event Func<DateTime> GetSpecifiedTillDateCallback;
        public event Action<IEnumerable<QueryResult>> QueryResultHandler;

        Context _context;

        public Queries(Context context)
        {
            _context = context;
        }

        public void ConductQuery(int queryID)
        {
            try
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
            catch
            { throw new NotImplementedException("Failed to conduct query"); }
        }

        public void GetQueryInfo()
        {
            List <QueryContainer> queryList = new List<QueryContainer>();
            queryList.Add(new QueryContainer(1, "Table Statistics"));
            queryList.Add(new QueryContainer(2, "Staff Statistics"));
            QueryCollectionHandler?.Invoke(queryList, "QueryID", "Description");
        }

        void TableStats()
        {
            DateTime fromDate;
            DateTime tillDate;

            try
            {
                fromDate = GetSpecifiedFromDateCallback.Invoke();
                tillDate = GetSpecifiedTillDateCallback.Invoke();
            }
            catch { throw new InvalidOperationException("Specify time period"); }

            var result = _context.Tables
                .Select(t => new QueryResult
                {
                    Name = t.Id.ToString(),
                    NumberOfOrders = t.RelatedOrders.Where(ro => ro.OrderTime >= fromDate && ro.OrderTime <= tillDate).Count(),
                    Profit = t.RelatedOrders
                        .Where(o => o.OrderTime >= fromDate && o.OrderTime <= tillDate)
                        .Select(o => new
                        {
                            ProfitPerOrder = o.OrderedDishes
                                .Select(d => new
                                {
                                    ProfitPerDish = (d.Dish.Price - d.Dish.Cost) * d.Quantity
                                })
                                .Sum(a => a.ProfitPerDish)
                        })
                        .Sum(a => a.ProfitPerOrder)
                });

            QueryResultHandler?.Invoke(result);
        }

        void StaffStats()
        {
            DateTime fromDate;
            DateTime tillDate;

            try
            {
                fromDate = GetSpecifiedFromDateCallback.Invoke();
                tillDate = GetSpecifiedTillDateCallback.Invoke();
            }
            catch { throw new InvalidOperationException("Specify time period"); }



            //QueryResultHandler?.Invoke(result);
        }
    }
}
