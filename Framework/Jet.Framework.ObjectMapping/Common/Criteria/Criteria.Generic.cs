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
    public partial class Criteria<T> : Criteria
    {
        public Criteria()
            : base(MappingService.Instance.GetTableMapping(typeof(T)))
        { }

        #region Order
        public new Criteria<T> AddOrderColumn(string column, SqlOrderType type = SqlOrderType.ASC)
        {
            base.AddOrderColumn(column, type);
            return this;
        }

        public Criteria<T> AddOrderProperty(Expression<Func<T, object>> expression, SqlOrderType type = SqlOrderType.ASC)
        {
            string prop = ExpressionHelper.GetExpressionMemberName(expression);
            ColumnMapping column = TableMapping.GetColumnMappingByProperty(prop);
            base.AddOrderColumn(column != null ? column.Name : prop, type);
            return this;
        }
        #endregion

        #region Column
        public new Criteria<T> AddColumn(string column)
        {
            base.AddColumn(column);
            return this;
        }

        public Criteria<T> AddProperty(Expression<Func<T, object>> expr)
        {
            var memberExpression = expr.Body as MemberExpression;
            if (memberExpression == null)
            {
                AnonymousObjectPropertiesExpressionVisitor Visitor = new AnonymousObjectPropertiesExpressionVisitor();
                Visitor.Visit(expr);
                foreach (var prop in Visitor.Properties)
                {
                    ColumnMapping column = TableMapping.GetColumnMappingByProperty(prop);
                    base.AddColumn(column != null ? column.Name : prop);
                }
            }
            else
            {
                string prop = memberExpression.Member.Name;
                ColumnMapping column = TableMapping.GetColumnMappingByProperty(prop);
                base.AddColumn(column != null ? column.Name : prop);
            }
            return this;
        }
        #endregion

        #region Update
        public new Criteria<T> AddUpdateColumn(string column, object val)
        {
            base.AddUpdateColumn(column, val);
            return this;
        }

        public Criteria<T> AddUpdateProperty(Expression<Func<T, object>> expression, object val)
        {
            string prop = ExpressionHelper.GetExpressionMemberName(expression);
            ColumnMapping column = TableMapping.GetColumnMappingByProperty(prop);
            this.UpdateParameters.Add(new SqlParameter(column != null ? column.Name : prop, val));
            return this;
        }

        public int Update()
        {
            DatabaseEngine db = DatabaseEngineFactory.GetDatabaseEngine();
            return db.Update(this);
        }

        public int Update(Expression<Func<object>> expression)
        {
            HashSet<string> props = ExpressionHelper.GetExpression_PropertyList(expression);

            if (props.Count == 0)
                throw new ObjectMappingException("not any property updated!");

            object obj = expression.Compile()();
            Type objType = obj.GetType();
            foreach (var prop in props)
            {
                var val = objType.GetProperty(prop).GetValue(obj, null);
                ColumnMapping column = TableMapping.GetColumnMappingByProperty(prop);
                this.UpdateParameters.Add(new SqlParameter(column != null ? column.Name : prop, val));
            }

            DatabaseEngine db = DatabaseEngineFactory.GetDatabaseEngine();
            return db.Update(this);
        }

        public new Criteria<T> AddUpdateCustom(string strsql, params SqlParameter[] paras)
        {
            base.AddUpdateCustom(strsql, paras);
            return this;
        }
        #endregion

        public new List<T> ToList()
        {
            return (List<T>)base.ToList();
        }

        public new T FirstOrDefault()
        {
            return (T)base.FirstOrDefault();
        }

        public T2 Max<T2>(Expression<Func<T, T2>> expression)
        {
            string prop = ExpressionHelper.GetExpressionMemberName(expression);
            ColumnMapping column = TableMapping.GetColumnMappingByProperty(prop);
            return (T2)base.Max(column != null ? column.Name : prop);
        }
    }
}
