using System;
using System.Collections.Generic;
using System.Linq;
using TableManagerData.Model;
using TableManageData;

namespace TableManagerData
{
    internal class TablesRepository : ITablesRepository
    {
        public event Action<int, string> TableInfoHandler;
        public event Action<IEnumerable<TableStatus>, string, string> TableStatusHandler;

        private Context _context;

        public TablesRepository(Context context)
        {
            _context = context;
        }
        

        //Defualt values written in SeedData embedded files
        //Table: Vacant 1, Occupied 2, Reserved 3
        //Order: Active 1, Completed 2
        public void ChangeStatus(int tableId, int statusId)
        {
            try
            {
                Table table = _context.Tables.SingleOrDefault(t => t.Id == tableId);

                if (table.RelatedOrders.FirstOrDefault(o => o.Status.Id == 1) != null)
                    throw new InvalidOperationException("There are active orders at the table right now. Can't change status");
                else
                    table.Status = _context.TableStatuses.Single(ts => ts.Id == statusId);
            }
            catch
            { throw new InvalidOperationException("Failed to change status"); }
        }


        public void GetTableInfo()
        {
            try
            {
                var statuses = _context.TableStatuses.AsEnumerable();
                //string valueProperty = typeof(TableStatus).GetProperties()[0].ToString();
                //string displayProperty = typeof(TableStatus).GetProperties()[1].ToString();
                TableStatusHandler?.Invoke(statuses, "Id", "Description");
                _context.Tables.ToList().ForEach(t => TableInfoHandler?.Invoke(t.NumberOfSeats, t.Location));
            }
            catch { throw new InvalidOperationException("Failed to retrieve table info"); }
        }
    }
}
