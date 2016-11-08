using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jet.Framework.ObjectMapping
{
    public enum SqlOrderType : int
    {
        ASC = 0,
        DESC = 1,
    }

    public class SqlOrder
    {
        public SqlOrder()
        {
        }

        public SqlOrder(string column, SqlOrderType type)
        {
            this.Column = column;
            this.Type = type;
        }

        public string Column { get; set; }

        public SqlOrderType Type { get; set; }
    }
}
