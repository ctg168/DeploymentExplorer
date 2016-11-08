using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jet.Framework.ObjectMapping
{
    public class SqlParameter
    {
        public SqlParameter()
        {
        }

        public SqlParameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }

        public object Value { get; set; }
    }
    public class SqlParameterCollection : List<SqlParameter>
    {
        public string GenerateParameter(object val)
        {
            string ret = string.Format("SmartPID_{0}", this.Count + 1);
            this.Add(new SqlParameter(ret, val));
            return ret;
        }
    }

}
