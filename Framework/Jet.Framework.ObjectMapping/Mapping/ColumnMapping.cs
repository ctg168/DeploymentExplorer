using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jet.Framework.ObjectMapping
{

    public class ColumnMapping
    {
        public string Name { get; set; }

        public bool IsPK { get; set; }

        public bool IsAutoIncrement { get; set; }

        public bool IsUseSeedFactory { get; set; }

        public bool IsViewColumn { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public Type ColumnType
        {
            get { return PropertyInfo.PropertyType; }
        }

        public string PropertyName
        {
            get { return PropertyInfo.Name; }
        }

        public DataInterceptAttribute DataIntercept { get; set; }

        public void SetValue(object obj, object val)
        {
            if (DataIntercept != null)
            {
                val = DataIntercept.SetValue(obj, val);
            }

            if (val != DBNull.Value)
            {
                object tempval;

                if (this.ColumnType.IsEnum)
                    tempval = Enum.ToObject(this.ColumnType, val);
                else if (this.ColumnType.IsValueType && (this.ColumnType.IsGenericType == false))
                    tempval = Convert.ChangeType(val, this.ColumnType);
                else
                    tempval = val;

                PropertyInfo.SetValue(obj, tempval, null);
            }
        }

        public object GetValue(object obj)
        {
            object val = PropertyInfo.GetValue(obj, null);
            if (DataIntercept != null)
                val = DataIntercept.GetValue(obj, val);
            return val;
        }
    }
}
