using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{


    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute()
        {

        }

        public ColumnAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
        public bool IsPK { get; set; }
        public bool IsAutoIncrement { get; set; }
        public bool IsUseSeedFactory { get; set; }
        public bool IsViewColumn { get; set; }
    }
}
