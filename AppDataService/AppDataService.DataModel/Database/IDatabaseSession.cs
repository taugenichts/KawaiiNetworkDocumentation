using System.Collections.Generic;
using System.Data;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public interface IDatabaseSession
    {
        string User { get; }

        IDbConnection GetConnection();

        IEnumerable<T> Query<T>(string sql, IDictionary<string, object> parameterList);

        int Insert<T>(string insertSql, T entity);

        void UpdateSingle(string updateSql, IDictionary<string, object> parameterList, IDataModel entity);
    }
}
