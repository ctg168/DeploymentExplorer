using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class DataInterceptAttribute : Attribute
    {
        internal ColumnMapping ColumnMapping { get; set; }

        public abstract object GetValue(object obj, object val);

        public abstract object SetValue(object obj, object val);
    }
}
