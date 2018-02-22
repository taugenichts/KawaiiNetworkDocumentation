using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Dapper;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class SqlSelect<T>
        where T : IDataModel, new()
    {
        private string tableName;
        private IEnumerable<TableColumnInfo> tableColumns;
        private int? selectNumberOfRecords;

        public SqlSelect()
        {
            var modelType = typeof(T);
            this.tableName = modelType.Name;

            var interfaceType = typeof(IDataModel);
            IReadOnlyCollection<string> interfaceProperties = interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Select(x => x.Name).ToList();

            var properties = modelType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            this.tableColumns = properties.Where(x => !interfaceProperties.Contains(x.Name)).Select(x => new TableColumnInfo(x));
        }

        public SqlSelect<T> Top(int top)
        {
            this.selectNumberOfRecords = top > 0 ? top : (int?)null;
            return this;
        }

        public IEnumerable<T> Run(IDatabaseSession dbSession, CommandBehavior behavior = CommandBehavior.Default)
        {
            IEnumerable<T> result = null;

            using (var connection = dbSession.GetConnection())
            {
                if(connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                result = connection.Query<T>(this.BuildSql());
            }

            return result;
        }

        private string BuildSql()
        {
            string topString = this.selectNumberOfRecords.HasValue ? string.Format("TOP {0} ", this.selectNumberOfRecords.Value) : string.Empty;

            return string.Format("SELECT {0}{1} FROM {2};",
                                    topString,
                                    string.Join(", ", this.tableColumns.Select(x => x.ColumnName)),
                                    this.tableName);
        }
    }
}
