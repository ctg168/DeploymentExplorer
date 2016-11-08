using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public TableAttribute()
        { }

        public TableAttribute(string tablename)
        {
            this.Name = tablename;
        }

        public string Name { get; set; }
    }
}
