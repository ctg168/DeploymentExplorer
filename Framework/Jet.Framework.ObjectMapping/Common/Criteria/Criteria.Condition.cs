using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jet.Framework.ObjectMapping
{
    public partial class Criteria
    {
        public Criteria ClearConditions()
        {
            this.Conditions.Clear();
            return this;
        }

        public Criteria AddConditionColumnValue(SqlConditionType type, string column, object val)
        {
            this.Conditions.Add(new SqlCondition()
            {
                Type = type,
                Column = column,
                Value = val
            });
            return this;
        }

        public Criteria AddConditionColumnValues(SqlConditionType type, string column, IList vals)
        {
            this.Conditions.Add(new SqlCondition()
            {
                Type = type,
                Column = column,
                Values = vals
            });
            return this;
        }

        public Criteria AddEqualTo(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.EqualTo, column, val);
        }

        public Criteria AddNotEqualTo(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.NotEqualTo, column, val);
        }

        public Criteria AddGreaterThan(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.GreaterThan, column, val);
        }

        public Criteria AddLessThan(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.LessThan, column, val);
        }

        public Criteria AddLessThanAndEqualTo(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.LessThanAndEqualTo, column, val);
        }

        public Criteria AddMatch(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.Match, column, val);
        }

        public Criteria AddMatchFullText(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.MatchFullText, column, val);
        }

        public Criteria AddMatchPrefix(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.MatchPrefix, column, val);
        }

        public Criteria AddMatchSuffix(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.MatchSuffix, column, val);
        }

        public Criteria AddNotMatch(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.NotMatch, column, val);
        }

        public Criteria AddNotMatchPrefix(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.NotMatchPrefix, column, val);
        }

        public Criteria AddNotMatchSuffix(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.NotMatchSuffix, column, val);
        }

        public Criteria AddIn(string column, IList vals)
        {
            return this.AddConditionColumnValues(SqlConditionType.In, column, vals);
        }

        public Criteria AddNotIn(string column, IList vals)
        {
            return this.AddConditionColumnValues(SqlConditionType.NotIn, column, vals);
        }

        public Criteria AddConditionCustom(string condition)
        {
            return this.AddConditionColumnValue(SqlConditionType.Custom, null, condition);
        }
    }
}
