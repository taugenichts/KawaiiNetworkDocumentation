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

        public int Insert<T>(string insertSql, T entity)
        {
            var insertSqlReturningId = string.Format("{0}; SELECT CAST(SCOPE_IDENTITY() as int)", insertSql);

            int serverId;

            using (var connection = this.GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                serverId = connection.Query<int>(insertSqlReturningId, entity).Single();
            }

            return serverId;
        }

        public void UpdateSingle(string updateSql, IDictionary<string, object> parameterList, IDataModel entity)
        {
            int affectedRows = 0;

            using (var connection = this.GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                affectedRows = connection.Execute(updateSql, parameterList);
            }

            if(affectedRows != 1)
            {
                DatabaseExceptionHelper.ThrowConcurrencyExcepiption(entity.Id);
            }
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