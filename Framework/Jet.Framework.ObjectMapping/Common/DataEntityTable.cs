using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jet.Framework.ObjectMapping
{
    public class DataEntityTable
    {
        public string Database { get; set; }
        public string TableName { get; set; }
        public string PkColumn { get; set; }
        public string SortColumn { get; set; }
        public string TableTypeName { get; set; }
        public List<DataEntityColumn> Columns { get; set; }
        public bool PkUseSeedFactory { get; set; }
        public bool PkAutoIncrement { get; set; }
    }

    public class DataEntityColumn
    {
        public string Name { get; set; }
        public string DataType { get; set; }
    }
}
