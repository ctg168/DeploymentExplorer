using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Jet.Framework.Utility
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public static class NotifyBaseExtend
    {
        public static void RaisePropertyChanged<T, TProperty>(this T notifyObj, Expression<Func<T, TProperty>> expression)
            where T : NotifyBase
        {
            notifyObj.RaisePropertyChanged(GetPropertyName(expression));
        }

        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;

                if (unaryExpression == null)
                {
                    throw new NotImplementedException();
                }

                memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression == null)
                {
                    throw new NotImplementedException();
                }
            }

            return memberExpression.Member.Name;
        }
    }
}
