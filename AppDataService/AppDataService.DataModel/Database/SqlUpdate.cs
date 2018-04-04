using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class SqlUpdate<T>
        where T : IDataModel
    {
        private const string LastRowVersionParameterName = "RowVersionOld";

        private readonly T entity;
        private readonly string tableName;
        private readonly string primaryKeyColumn;
        private readonly IEnumerable<string> columnNames;

        public SqlUpdate(T entity)
        {
            this.entity = entity;
            var modelType = typeof(T);
            this.tableName = modelType.Name;
            this.primaryKeyColumn = DataModelHelper.GetPrimaryKeyProperty(modelType);
            this.columnNames = DataModelHelper.GetProperties(modelType).Where(x => x != primaryKeyColumn);
        }

        public UpdatedResponse Run(IDatabaseSession dbSession)
        {            
            var modificationTimeStamp = DateTime.Now;
            IDictionary<string, object> parameters;
            var changeInfo = entity as IRecordChangeInfo;
            var updateSql = this.BuildSql(changeInfo != null);

            if (changeInfo != null)
            {                                   
                parameters = this.GetParameters(changeInfo.RowVersion);                           
            }
            else
            {
                parameters = this.GetParameters();
            }            

            var record = dbSession.UpdateSingle(updateSql, parameters, this.entity);

            return new UpdatedResponse()
                        {
                            ServerId = this.entity.Id,
                            RowVersion = record.RowVersion
                        };
        }      
        
        private string BuildSql(bool checkConcurrency)
        {
            var updateSqlBuilder = new StringBuilder();

            updateSqlBuilder.Append(string.Format("UPDATE {0} SET {1} WHERE {2}",
                                        this.tableName,
                                        string.Join(", ", this.columnNames.Select(x => string.Format("{0} = @{0}", x))),
                                        string.Format("{0} = @{0}", this.primaryKeyColumn)));

            if (checkConcurrency)
            {
                updateSqlBuilder.Append(string.Format(" AND {0} = @{1}",
                                                DataModelHelper.RowVersionPropertyName,
                                                LastRowVersionParameterName));
            }

            return updateSqlBuilder.ToString();
        }

        private IDictionary<string, object> GetParameters(byte[] lastRowVersion = null)
        {
            var parameters = new Dictionary<string, object>();
            var properties = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

            // primary key
            parameters.Add(this.primaryKeyColumn, properties.Single(x => x.Name == this.primaryKeyColumn).GetValue(this.entity));

            // every other column
            foreach(var column in this.columnNames)
            {
                parameters.Add(column, properties.Single(x => x.Name == column).GetValue(this.entity));
            }

            // add concurrency parameters if necessary
            if(lastRowVersion != null)
            {
                parameters.Add(LastRowVersionParameterName, lastRowVersion);
            }

            return parameters;
        }
    }
}
