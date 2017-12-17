using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManagerData.Model;

namespace TableManagerData
{
    public interface ITablesRepository
    {
        /// <summary>
        /// Invoked for each table instance in GetTableInfo().
        /// Assign to get id, number of seats, table's status id, X and Y coordinates of a table (order dependent)
        /// </summary>
        event Action<int, int, int, int, int> TableInfoHandler;

        /// <summary>
        /// Invoked in GetTableInfo(). 
        /// Assign to get collection of table statuses, SelectedValuePath (status id), and DisplayMemberPath (status descripion) 
        /// </summary>
        event Action<IEnumerable<TableStatus>, string, string> TableStatusHandler;

        /// <summary>
        /// Invoked on cancelation and completion of an order. Assign to get id of the table to be updated
        /// </summary>
        event Action<int> UpdateTableByIdHandler;

        /// <summary>
        /// Use to get change table status
        /// <param name="tableId">table's id whose status is changed</param>
        /// <param name="statusId">id should be passed from earlier retrieved status collection. Use TableStatusHandler for this</param>  
        /// </summary>
        void ChangeStatus(int tableId, int statusId);

        /// <summary>
        /// Call for get all table related infromation. Invokes TableInfoHandler and TableStatusHandler
        /// </summary>
        void GetTableInfo();

        /// <summary>
        /// Reserves a table or cancel its reservation if it's already reserved
        /// </summary>
        /// <param name="tableId"></param>
        void ReserveOrCancelReservation(int tableId);

        /// <summary>
        /// Gets status id of the table
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        int GetTableStatusId(int tableId);
    }
}
