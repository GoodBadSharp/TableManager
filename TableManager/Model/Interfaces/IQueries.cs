using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableManagerData.Interfaces
{
    public interface IQueries
    {
        /// <summary>
        /// Conducts query to a database
        /// </summary>
        /// <param name="queryId">Id of the query. Can be retrieved using QueryCollectionHandler</param>
        void ConductQuery(int queryId);
    }
}
