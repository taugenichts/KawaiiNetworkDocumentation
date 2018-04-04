using System;
using System.Collections.Generic;
using System.Linq;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class SqlInsert<T>
        where T : IDataModel
    {
        private readonly string tableName;
        private readonly string primaryKeyColumn;
        private readonly IEnumerable<string> columnNames;
        private readonly T entity;

        public SqlInsert(T entity)
        {
            this.entity = entity;
            var modelType = typeof(T);
            this.tableName = modelType.Name;
            this.primaryKeyColumn = DataModelHelper.GetPrimaryKeyProperty(modelType);
            this.columnNames = DataModelHelper.GetProperties(modelType).Where(x => x != this.primaryKeyColumn);            
        }

        public CreatedResponse Run(IDatabaseSession dbSession)
        {
            var clientId = entity.Id;
            var recordChanges = entity as IRecordChangeInfo;            

            var newRecord = dbSession.Insert<T>(this.BuildSql(), entity);

            return new CreatedResponse
                        {
                            ClientId = clientId,
                            ServerId = newRecord.Id,
                            RowVersion = newRecord.RowVersion
                        };
        }

        private string BuildSql()
        {
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                            this.tableName,
                            string.Join(", ", this.columnNames),
                            string.Join(", ", this.columnNames.Select(x => string.Format(@"@{0}", x))));
        }
    }
}
