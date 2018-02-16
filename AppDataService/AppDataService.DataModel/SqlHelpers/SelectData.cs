using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Kawaii.NetworkDocumentation.AppDataService.DataModel;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.SqlHelpers
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

            var properties = modelType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            this.tableColumns = properties.Select(x => new TableColumnInfo(x));
        }

        public SqlSelect<T> Top(int top)
        {
            this.selectNumberOfRecords = top > 0 ? top : (int?)null;
            return this;
        }

        public IEnumerable<T> Execute(IDbConnection connection, CommandBehavior behavior = CommandBehavior.Default)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = this.BuildSql();

            var reader = cmd.ExecuteReader(behavior);
            var result = new List<T>();

            while (reader.Read())
            {
                var item = new T();

                foreach (var column in this.tableColumns)
                {
                    column.ModelPropertyInfo.SetValue(item, reader[column.ColumnName]);
                }

                result.Add(item);
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
