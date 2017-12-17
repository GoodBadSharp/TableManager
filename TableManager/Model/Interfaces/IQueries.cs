using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableManagerData.QueryLogic;

namespace TableManagerData.Interfaces
{
    public interface IQueries
    {
        /// <summary>
        /// Returns headers and bindings of QueryResult for a table
        /// </summary>
        event Action<IEnumerable<string>, IEnumerable<string>> UpdateTableHeadersHandler;

        /// <summary>
        /// Invoked in ConductQuery.
        /// Assign to get collection of statistics types, SelectedValuePath (query id), and DisplayMemberPath (query descripion)
        /// </summary>
        event Action<IEnumerable<QueryContainer>, string, string> QueryCollectionHandler;

        /// <summary>
        /// Use to pass the beginning of a time period for a query. If not specified, earliest order's date will be taken
        /// </summary>
        event Func<DateTime?> GetSpecifiedFromDateCallback;

        /// <summary>
        /// Use to pass the end of a time period for a query. If not specified, latest order's date will be taken
        /// </summary>
        event Func<DateTime?> GetSpecifiedTillDateCallback;

        /// <summary>
        /// Returns generic query result
        /// </summary>
        event Action<IEnumerable<QueryResult>> QueryResultHandler;

        /// <summary>
        /// Conducts query to a database
        /// </summary>
        /// <param name="queryId">Id of the query. Can be retrieved using QueryCollectionHandler</param>
        void ConductQuery(int queryId);

        /// <summary>
        /// Use to get a collection of available statistics queries. Get them through invoked QueryCollectionHandler
        /// </summary>
        IEnumerable<QueryContainer> GetQueryInfo();
    }
}
