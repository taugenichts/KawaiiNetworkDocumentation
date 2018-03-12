using System.Text;

namespace Kawaii.NetworkDocumentation.AppDataService.DataModel.Database
{
    public class SqlCondition : SqlConditionBase
    {
        private string valueParameterName = string.Empty;

        public SqlCondition(LogicalOperator appendOperator = LogicalOperator.And)
        {
        }

        public SqlCondition(string propertyName, ComparisionOperator comparisionOperator, object value)
        {            
            this.AppendWithOperator = LogicalOperator.And;
            this.PropertyName = propertyName;
            this.ComparisionOperator = comparisionOperator;
            this.Value = value;
            this.initiallyfixLikeValue();
        }

        public SqlCondition(string propertyName, ComparisionOperator comparisionOperator, object value, LogicalOperator logicalOperator)
        {
            this.AppendWithOperator = logicalOperator;
            this.PropertyName = propertyName;
            this.ComparisionOperator = comparisionOperator;
            this.Value = value;
            this.initiallyfixLikeValue();
        }

        public SqlCondition(string propertyName, ComparisionOperator comparisionOperator, object value, LogicalOperator logicalOperator, string parameterName)
        {
            this.AppendWithOperator = logicalOperator;
            this.PropertyName = propertyName;
            this.ComparisionOperator = comparisionOperator;
            this.Value = value;
            this.ValueParameterName = parameterName;
            this.initiallyfixLikeValue();
        }

        public string PropertyName { get; set; }

        public ComparisionOperator ComparisionOperator { get; set; }

        public object Value { get; set; }

        public string ValueParameterName
        {
            get
            {
                if (string.IsNullOrEmpty(this.valueParameterName))
                {
                    this.valueParameterName = this.buildParameterName(this.PropertyName);
                }

                return this.valueParameterName;
            }

            set
            {
                this.valueParameterName = value;
            }
        }

        public override string ToString()
        {
            string leftOperand = this.PropertyName;
            string compOperator = this.ComparisionOperator == ComparisionOperator.Equals ? "="
                                    : this.ComparisionOperator == ComparisionOperator.NotEquals ? "<>"
                                    : this.ComparisionOperator == ComparisionOperator.IsNull ? "IS NULL"
                                    : this.ComparisionOperator == ComparisionOperator.IsNotNull ? "IS NOT NULL"
                                    : this.ComparisionOperator == ComparisionOperator.Like ? "LIKE" : "=";
            string rightOperand = (this.ComparisionOperator != ComparisionOperator.IsNull
                                    && this.ComparisionOperator != ComparisionOperator.IsNotNull)
                                    ? "@" + this.ValueParameterName : string.Empty;

            return string.Format("({0} {1} {2})", leftOperand, compOperator, rightOperand);                                    
        }

        private string buildParameterName(string propertyName)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in propertyName)
            {
                if ((c >= 'a' && c <= 'z')
                    || (c >= 'A' && c <= 'Z')
                    || (c >= '0' && c <= '9'))
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }

        private void initiallyfixLikeValue()
        {
            if(this.Value is string
                && this.ComparisionOperator == ComparisionOperator.Like
                && !((string)this.Value).EndsWith("%"))
            {
                this.Value = ((string)this.Value) + "%";
            }
        }
    }

    public enum ComparisionOperator
    {
        Equals = 1,
        NotEquals = 2,
        IsNull = 3,
        IsNotNull = 4,
        Like = 5
    }
}
