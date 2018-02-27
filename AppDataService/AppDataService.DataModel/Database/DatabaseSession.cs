using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class DatabaseSession : IDatabaseSession
    {
        
        private Func<string, IDbConnection> createConnectionDelegate;
        private string connectionString;

        public DatabaseSession(string connectionString, Func<string, IDbConnection> createConnection)
        {
            this.connectionString = connectionString;
            this.createConnectionDelegate = createConnection;
        }

        public IDbConnection GetConnection()
        {
            return this.createConnectionDelegate(this.connectionString);
        }

        public IEnumerable<T> Query<T>(string sql)
        {
            IEnumerable<T> results = null;

            using(var connection = this.GetConnection())
            {
                if(connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                results = connection.Query<T>(sql);
            }

            return results ?? new List<T>();
        }
    }
}