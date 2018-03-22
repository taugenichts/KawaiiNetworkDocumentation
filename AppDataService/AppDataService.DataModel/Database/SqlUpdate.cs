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
        private const string LastModifiedConcurrencyParameterName = "LastModifiedOld";
        private const string LastModifiedByConcurrencyParameterName = "LastModifiedByOld";

        private readonly T entity;
        private readonly string tableName;
        private readonly string primaryKeyColumn;
        private readonly IEnumerable<string> columnNames;

        public SqlUpdate(T entity)
        {
            this.entity = entity;
            var modelType = typeof(T);
            this.tableName = modelType.Name;
            this.primaryKeyColumn = this.tableName + "Id";
            this.columnNames = DataModelHelper.GetProperties(modelType).Where(x => x != this.primaryKeyColumn);
        }

        public UpdatedResponse Run(IDatabaseSession dbSession)
        {            
            var modificationTimeStamp = DateTime.Now;
            IDictionary<string, object> parameters;
            var changeInfo = entity as IRecordChangeInfo;
            var updateSql = this.BuildSql(changeInfo != null);

            if (changeInfo != null)
            {                
                var oldLastModified = changeInfo.LastModified;
                var oldLastModifiedBy = changeInfo.LastModifiedBy;
                changeInfo.LastModified = modificationTimeStamp;
                changeInfo.LastModifiedBy = dbSession.User;
                                
                parameters = this.GetParameters(oldLastModified, oldLastModifiedBy);                           
            }
            else
            {
                parameters = this.GetParameters();
            }            

            dbSession.UpdateSingle(updateSql, parameters, this.entity);

            return new UpdatedResponse()
                        {
                            ServerId = this.entity.Id,
                            LastModified = modificationTimeStamp,
                            LastModifiedBy = dbSession.User
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
                updateSqlBuilder.Append(string.Format(" AND {0} = @{1} AND {2} = @{3}",
                                                DataModelHelper.LastModifiedProperty,
                                                LastModifiedConcurrencyParameterName,
                                                DataModelHelper.LastModifiedByProperty,
                                                LastModifiedByConcurrencyParameterName));
            }

            return updateSqlBuilder.ToString();
        }

        private IDictionary<string, object> GetParameters(DateTime? lastModified = null, string lastModifiedBy = null)
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
            if(lastModified.HasValue && !string.IsNullOrEmpty(lastModifiedBy))
            {
                parameters.Add(LastModifiedConcurrencyParameterName, lastModified.Value);
                parameters.Add(LastModifiedByConcurrencyParameterName, lastModifiedBy);
            }

            return parameters;
        }
    }
}
