using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jet.Framework.ObjectMapping
{
    public class JsonColumnAttribute : DataInterceptAttribute
    {
        public override object GetValue(object obj, object val)
        {
            object ret = null;
            if(val != null)
                ret = JsonConvert.SerializeObject(val);
            return ret;
        }

        public override object SetValue(object obj, object val)
        {
            object ret = null;
            if (val != DBNull.Value)
            {
                String str = val as String;
                if(!String.IsNullOrEmpty(str))
                    ret = JsonConvert.DeserializeObject(str, this.ColumnMapping.PropertyInfo.PropertyType);
            }
            return ret;
        }
    }
}
