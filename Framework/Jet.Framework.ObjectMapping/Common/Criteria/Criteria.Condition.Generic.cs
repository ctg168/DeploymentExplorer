using Jet.Framework.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jet.Framework.ObjectMapping
{
    public partial class Criteria<T>
    {
        public new Criteria<T> ClearConditions()
        {
            base.ClearConditions();
            return this;
        }

        public new Criteria<T> AddConditionColumnValue(SqlConditionType type, string column, object val)
        {
            base.AddConditionColumnValue(type, column, val);
            return this;
        }

        public new Criteria<T> AddConditionColumnValues(SqlConditionType type, string column, IList vals)
        {
            base.AddConditionColumnValues(type, column, vals);
            return this;
        }

        public Criteria<T> AddConditionPropertyValue(SqlConditionType type, string prop, object val)
        {
            ColumnMapping column = TableMapping.GetColumnMappingByProperty(prop);
            this.AddConditionColumnValue(type, column != null ? column.Name : prop, val);
            return this;
        }

        public Criteria<T> AddConditionPropertyValues(SqlConditionType type, string prop, IList vals)
        {
            ColumnMapping column = TableMapping.GetColumnMappingByProperty(prop);
            this.AddConditionColumnValues(type, column != null ? column.Name : prop, vals);
            return this;
        }

        public Criteria<T> AddEqualTo(Expression<Func<T, object>> expression, object val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.EqualTo, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddEqualTo(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.EqualTo, column, val);
        }

        public Criteria<T> AddNotEqualTo(Expression<Func<T, object>> expression, object val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.NotEqualTo, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddNotEqualTo(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.NotEqualTo, column, val);
        }

        public Criteria<T> AddGreaterThan(Expression<Func<T, object>> expression, object val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.GreaterThan, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddGreaterThan(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.GreaterThan, column, val);
        }

        public Criteria<T> AddLessThan(Expression<Func<T, object>> expression, object val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.LessThan, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddLessThan(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.LessThan, column, val);
        }

        public Criteria<T> AddGreaterThanEqualTo(Expression<Func<T, object>> expression, object val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.GreaterThanAndEqualTo, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddLessThanAndEqualTo(string column, object val)
        {
            return this.AddConditionColumnValue(SqlConditionType.LessThanAndEqualTo, column, val);
        }

        public Criteria<T> AddMatch(Expression<Func<T, object>> expression, string val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.Match, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddMatch(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.Match, column, val);
        }

        public Criteria<T> AddMatchFullText(Expression<Func<T, object>> expression, string val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.MatchFullText, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddMatchFullText(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.MatchFullText, column, val);
        }

        public Criteria<T> AddMatchPrefix(Expression<Func<T, object>> expression, string val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.MatchPrefix, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddMatchPrefix(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.MatchPrefix, column, val);
        }

        public Criteria<T> AddMatchSuffix(Expression<Func<T, object>> expression, string val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.MatchSuffix, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddMatchSuffix(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.MatchSuffix, column, val);
        }

        public Criteria<T> AddNotMatch(Expression<Func<T, object>> expression, string val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.NotMatch, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddNotMatch(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.NotMatch, column, val);
        }

        public Criteria<T> AddNotMatchPrefix(Expression<Func<T, object>> expression, string val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.NotMatchPrefix, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddNotMatchPrefix(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.NotMatchPrefix, column, val);
        }

        public Criteria<T> AddNotMatchSuffix(Expression<Func<T, object>> expression, string val)
        {
            return this.AddConditionPropertyValue(SqlConditionType.NotMatchSuffix, ExpressionHelper.GetExpressionMemberName(expression), val);
        }

        public new Criteria<T> AddNotMatchSuffix(string column, string val)
        {
            return this.AddConditionColumnValue(SqlConditionType.NotMatchSuffix, column, val);
        }

        public Criteria<T> AddIn(Expression<Func<T, object>> expression, IList vals)
        {
            return this.AddConditionPropertyValues(SqlConditionType.In, ExpressionHelper.GetExpressionMemberName(expression), vals);
        }

        public new Criteria<T> AddIn(string column, IList vals)
        {
            return this.AddConditionColumnValues(SqlConditionType.In, column, vals);
        }

        public Criteria<T> AddNotIn(Expression<Func<T, object>> expression, IList vals)
        {
            return this.AddConditionPropertyValues(SqlConditionType.NotIn, ExpressionHelper.GetExpressionMemberName(expression), vals);
        }

        public new Criteria<T> AddNotIn(string column, IList vals)
        {
            return this.AddConditionColumnValues(SqlConditionType.NotIn, column, vals);
        }

        public new Criteria<T> AddConditionCustom(string condition)
        {
            return this.AddConditionColumnValue(SqlConditionType.Custom, null, condition);
        }
    }
}
