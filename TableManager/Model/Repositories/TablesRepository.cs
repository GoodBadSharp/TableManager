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
                    _context.SaveChanges();
                }
                else
                {
                    if (table.Status_Id == 3)
                    {
                        ChangeStatus(tableId, 1);
                        _context.SaveChanges();
                    }
                    else {
                        throw new InvalidOperationException(
                            "There are active orders at the table right now. Can't change status"); }
                }
            }
        }

        public int GetTableStatusId(int tableId)
        {
            try
            {
                return _context.Tables.Single(t => t.Id == tableId).Status_Id;
            }
            catch (Exception)
            {
                throw new Exception("Table doesn't exist");
            }
        }
        
        public void ChangeStatus(int tableId, int statusId)
        {
            try
            {
                Table table = _context.Tables.SingleOrDefault(t => t.Id == tableId);

                if (table.RelatedOrders == null)
                {
                    table.Status = _context.TableStatuses.SingleOrDefault(s => s.Id == statusId);
                    _context.SaveChanges();
                }
                else
                {
                    if (table.RelatedOrders.FirstOrDefault(o => o.Status.Id == 1) != null)
                        throw new InvalidOperationException("There are active orders at the table right now. Can't change status");
                    else
                    {
                        table.Status = _context.TableStatuses.SingleOrDefault(s => s.Id == statusId);
                        _context.SaveChanges();
                    }
                }
            }
            catch
            { throw new InvalidOperationException("Failed to change status"); }
        }


        public void GetTableInfo()
        {
            //try
            //{
                var statuses = _context.TableStatuses.AsNoTracking();
                //string valueProperty = typeof(TableStatus).GetProperties()[0].ToString();
                //string displayProperty = typeof(TableStatus).GetProperties()[1].ToString();
                TableStatusHandler?.Invoke(statuses, "Id", "Description");
                _context.Tables.ToList()
                    .ForEach(t => TableInfoHandler?.Invoke(t.Id, t.NumberOfSeats, t.Status_Id, t.X, t.Y));
            //}
            //catch { throw new InvalidOperationException("Failed to retrieve table info"); }
        }
    }
}
