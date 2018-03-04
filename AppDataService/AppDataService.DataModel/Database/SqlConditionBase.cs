namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class SqlConditionBase
    {
        public LogicalOperator AppendWithOperator { get; set; }

        public SqlConditionBase(LogicalOperator appendOperator = LogicalOperator.And)
        {
            this.AppendWithOperator = appendOperator;
        }
    }

    public enum LogicalOperator
    {
        And = 1,
        Or = 2
    }
}
