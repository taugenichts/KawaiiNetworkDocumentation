using System.Collections.Generic;
using System.Data;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public interface IDatabaseSession
    {
        IDbConnection GetConnection();

        IEnumerable<T> Query<T>(string sql, IDictionary<string, object> parameterList);
    }
}
