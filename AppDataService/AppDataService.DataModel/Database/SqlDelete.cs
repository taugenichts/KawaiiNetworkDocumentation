using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class SqlDelete<T>
        where T : IDataModel
    {
        private const string LastRowVersionParameterName = "RowVersionOld";

        private readonly T entity;
        private readonly string tableName;
        private readonly string primaryKeyColumn;        

        public SqlDelete(T entity)
        {
            this.entity = entity;
            var modelType = typeof(T);
            this.tableName = modelType.Name;
            this.primaryKeyColumn = DataModelHelper.GetPrimaryKeyProperty(modelType);            
        }

        public DeletedResponse Run(IDatabaseSession dbSession)
        {
            IDictionary<string, object> parameters;
            var changeInfo = entity as IRecordChangeInfo;
            var deleteSql = BuildSql(changeInfo != null);

            parameters = this.GetParameters(changeInfo?.RowVersion);

            dbSession.DeleteSingle(deleteSql, parameters, this.entity);

            return new DeletedResponse()
            {
                Success = true
            };
        }

        private string BuildSql(bool checkConcurrency)
        {
            var deleteSqlBuilder = new StringBuilder();

            deleteSqlBuilder.Append(string.Format("DELETE FROM {0} WHERE {1} = @{1}",
                                        this.tableName,                                        
                                        this.primaryKeyColumn));

            if (checkConcurrency)
            {
                deleteSqlBuilder.Append(string.Format(" AND {0} = @{1}",
                                                DataModelHelper.RowVersionPropertyName,
                                                LastRowVersionParameterName));
            }

            return deleteSqlBuilder.ToString();
        }

        private IDictionary<string, object> GetParameters(byte[] lastRowVersion = null)
        {
            var parameters = new Dictionary<string, object>();
            
            // primary key
            parameters.Add(this.primaryKeyColumn, this.entity.Id);
            
            // add concurrency parameters if necessary
            if (lastRowVersion != null)
            {
                parameters.Add(LastRowVersionParameterName, lastRowVersion);
            }

            return parameters;
        }
    }
}
