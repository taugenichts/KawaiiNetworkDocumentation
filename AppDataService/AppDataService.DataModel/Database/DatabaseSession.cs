using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using Dapper;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class DatabaseSession : IDatabaseSession
    {        
        private Func<string, IDbConnection> createConnectionDelegate;
        private string connectionString;
        
        public string User
        {
            get
            {                
                return string.IsNullOrEmpty(HttpContext.Current?.User?.Identity?.Name) ?
                            "anonymous"
                            : HttpContext.Current?.User?.Identity?.Name;                
            }
        }

        public DatabaseSession(string connectionString, Func<string, IDbConnection> createConnection)
        {
            this.connectionString = connectionString;
            this.createConnectionDelegate = createConnection;
        }

        public IDbConnection GetConnection()
        {
            return this.createConnectionDelegate(this.connectionString);
        }

        public IEnumerable<T> Query<T>(string sql, IDictionary<string, object> parameterList)
        {
            IEnumerable<T> results = null;

            using(var connection = this.GetConnection())
            {
                if(connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                var parameters = this.CreateDynamicParameters(parameterList);

                results = connection.Query<T>(sql, parameters);
            }

            return results ?? new List<T>();
        }

        public dynamic Insert<T>(string insertSql, T entity) where T : IDataModel
        {
            var modelType = typeof(T);
            var tableName = modelType.Name;
            var idColumn = DataModelHelper.GetPrimaryKeyProperty(modelType);

            var insertSqlReturningId = string.Format("{0}; SELECT {2} AS Id, RowVersion FROM {1} WHERE {2} = CAST(SCOPE_IDENTITY() as int)", insertSql, tableName, idColumn);

            dynamic record = null;

            using (var connection = this.GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                record = connection.Query(insertSqlReturningId, entity).Single();
            }

            return record;
        }

        public dynamic UpdateSingle<T>(string updateSql, IDictionary<string, object> parameterList, T entity) where T : IDataModel
        {
            var modelType = typeof(T);
            var tableName = modelType.Name;
            var idColumn = DataModelHelper.GetPrimaryKeyProperty(modelType);
                        
            dynamic record = null;

            using (var connection = this.GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                var affectedRows = connection.Execute(updateSql, parameterList);
                if(affectedRows == 0)
                {
                    DatabaseExceptionHelper.ThrowConcurrencyExcepiption(entity.Id);
                }

                var idQuery = string.Format("SELECT {1} AS Id, RowVersion FROM {0} WHERE {1} = @{1}", tableName, idColumn);
                record = connection.Query(idQuery, entity).Single();
            }

            return record;
        }

        private DynamicParameters CreateDynamicParameters(IDictionary<string, object> parameters)
        {
            if(parameters != null && parameters.Keys.Any())
            {
                var dynParams = new DynamicParameters();

                foreach(var key in parameters.Keys)
                {
                    dynParams.Add(string.Format("@{0}", key), parameters[key]);
                }

                return dynParams;
            }

            return null;
        }
    }
}