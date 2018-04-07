using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class SqlConditionGroup : SqlConditionBase
    {
        public SqlConditionGroup(LogicalOperator appendOperator = LogicalOperator.And) 
            : base(appendOperator)
        {
            this.ChildConditions = new List<SqlConditionBase>();
        }

        public IList<SqlConditionBase> ChildConditions { get; set; }

        public override string ToString()
        {
            // Ignore the AppendWithOperator property. 
            // It must be considered one level above
            StringBuilder builder = new StringBuilder();
            builder.Append('(');

            for (int i = 0; i < this.ChildConditions.Count(); i++)
            {
                var condition = this.ChildConditions[i];
                if(i == 0)
                {
                    builder.Append(string.Format("{0} ", condition.ToString()));
                }
                else
                {
                    builder.Append(string.Format("{0} {1} ", condition.AppendWithOperator.ToString(), condition.ToString()));
                }
            }

            builder.Append(')');

            return builder.ToString();
        }
    }
}
