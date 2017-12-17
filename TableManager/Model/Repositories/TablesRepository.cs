using System;
using System.Collections.Generic;
using System.Linq;
using TableManagerData.Model;
using TableManageData;

namespace TableManagerData
{
    internal class TablesRepository : ITablesRepository
    {
        public event Action<int, int, int, int, int> TableInfoHandler;
        public event Action<IEnumerable<TableStatus>, string, string> TableStatusHandler;
        public event Action<int> UpdateTableByIdHandler;

        private Context _context;

        public TablesRepository(Context context)
        {
            _context = context;
        }
        

        //Defualt values written in SeedData embedded files
        //Table: Vacant 1, Occupied 2, Reserved 3
        //Order: Active 1, Completed 2
        public void ReserveOrCancelReservation(int tableId)
        {
            Table table;
            if(_context.Tables.SingleOrDefault(t => t.Id == tableId) != null)
            {
                table = _context.Tables.Single(t => t.Id == tableId);
                if (table.Status_Id == 1)
                {
                    ChangeStatus(table.Id, 3);
                }
                else
                {
                    if (table.Status_Id == 3)
                    {
                        ChangeStatus(tableId, 1);
                    }
                    else
                    {
                        throw new InvalidOperationException("There are active orders at the table right now. Can't change status");
                    }
                }
            }
        }

        public int GetTableStatusId(int tableId)
        {
            try
            {
                return _context.Tables.Single(t => t.Id == tableId).Status_Id;
            }
            catch { throw new InvalidOperationException("Table wasn't found"); }
        }
        
        public void ChangeStatus(int tableId, int statusId)
        {
            try
            {
                Table table = _context.Tables.SingleOrDefault(t => t.Id == tableId);

                if (table.RelatedOrders == null)
                {
                    table.Status = _context.TableStatuses.SingleOrDefault(s => s.Id == statusId);
                }
                else
                {
                    if (table.RelatedOrders.FirstOrDefault(o => o.Status.Id == 1) != null)
                        throw new InvalidOperationException("There are active orders at the table right now. Can't change status");
                    else
                    {
                        table.Status = _context.TableStatuses.SingleOrDefault(s => s.Id == statusId);
                    }
                }
            }
            catch
            { throw new InvalidOperationException("Failed to change status"); }
        }

        public IEnumerable<Waiter> GetWaiterInfo()
        {
            return _context.Waiters.AsNoTracking();
        }

        public IEnumerable<Table> GetTableInfo()
        {
            return _context.Tables.AsNoTracking();
        }
    }
}
