using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class ExpressionHelper
    {
        /// <summary>
        /// 解析表达式"o => new {o.Prop1,o.Prop2,o.Prop3.....}" 的MemberName
        /// </summary>
        public static HashSet<string> GetExpression_PropertyList(Expression<Func<object>> source)
        {
            HashSet<string> list = new HashSet<string>();
            if (source.Body is MemberInitExpression)
            {
                MemberInitExpression memberInitExpression = source.Body as MemberInitExpression;
                foreach (MemberBinding memberBinding in memberInitExpression.Bindings)
                {
                    list.Add(memberBinding.Member.Name);
                }
            }
            return list;
        }

        public static string GetExpressionMemberName<T, T2>(Expression<Func<T, T2>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                    memberExpression = unaryExpression.Operand as MemberExpression;
            }
            if (memberExpression == null)
                throw new Exception("not a member expression");
            return memberExpression.Member.Name;
        }
    }

    public class AnonymousObjectPropertiesExpressionVisitor : ExpressionVisitor
    {
        public AnonymousObjectPropertiesExpressionVisitor()
        {
            Properties = new HashSet<string>();
        }

        public HashSet<string> Properties { get; private set; }

        protected override Expression VisitNew(NewExpression node)
        {
            foreach (var member in node.Members)
            {
                Properties.Add(member.Name);
            }
            return base.VisitNew(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            Properties.Add(node.Member.Name);

            return base.VisitMember(node);
        }
    }
}
