using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class SqlSelect<T>
        where T : IDataModel, new()
    {
        private string tableName;
        private IEnumerable<TableColumnInfo> tableColumns;
        private int? selectNumberOfRecords;
        private SqlConditionGroup conditions = new SqlConditionGroup();

        public SqlSelect()
        {
            var modelType = typeof(T);
            this.tableName = modelType.Name;

            var interfaceType = typeof(IDataModel);
            IReadOnlyCollection<string> interfaceProperties = interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Select(x => x.Name).ToList();

            var properties = modelType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            this.tableColumns = properties.Where(x => !interfaceProperties.Contains(x.Name)).Select(x => new TableColumnInfo(x));
        }

        public SqlSelect<T> First(int first)
        {
            this.selectNumberOfRecords = first > 0 ? first : (int?)null;
            return this;
        }

        public SqlSelect<T> AddCondition(SqlConditionBase condition)
        {
            this.conditions.ChildConditions.Add(condition);
            return this;
        }

        public SqlSelect<T> AddCondition(string propertyName, ComparisionOperator comparisionOperator, object value, LogicalOperator logicalOperator = LogicalOperator.And)
        {
            if(comparisionOperator != ComparisionOperator.IsNull 
                && comparisionOperator != ComparisionOperator.IsNotNull
                && value == null)
            {
                // if there is no meaningful value given, just skip adding this condition
                return this;
            }

            this.AddCondition(new SqlCondition(propertyName, comparisionOperator, value, logicalOperator));
            return this;
        }

        public IEnumerable<T> Run(IDatabaseSession dbSession)
        {
            var parameters = this.GetParameters();
            return dbSession.Query<T>(this.BuildSql(), parameters);
        }

        private string BuildSql()
        {
            string topString = this.selectNumberOfRecords.HasValue ? string.Format("TOP {0} ", this.selectNumberOfRecords.Value) : string.Empty;

            string whereString = this.conditions.ChildConditions.Any() ? string.Format(" WHERE {0}", this.conditions.ToString()) : string.Empty;

            return string.Format("SELECT {0}{1} FROM {2}{3};",
                                    topString,
                                    string.Join(", ", this.tableColumns.Select(x => x.ColumnName)),
                                    this.tableName,
                                    whereString);
        }

        private IDictionary<string, object> GetParameters()
        {
            if (conditions.ChildConditions.Any())
            {
                var parameters = new Dictionary<string, object>();
                FillParameterDictionaryRecursive(this.conditions, parameters);

                return parameters;
            }

            return null;
        }

        private static void FillParameterDictionaryRecursive(SqlConditionBase condition, IDictionary<string, object> dict)
        {
            if(condition is SqlCondition)
            {
                var singleCondition = condition as SqlCondition;
                dict.Add(singleCondition.ValueParameterName, singleCondition.Value);
            }
            else if(condition is SqlConditionGroup)
            {
                var conditionGroup = condition as SqlConditionGroup;
                foreach(var childCondition in conditionGroup.ChildConditions)
                {
                    FillParameterDictionaryRecursive(childCondition, dict);
                }
            }
        }
    }
}
