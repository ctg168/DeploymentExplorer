using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public class TableMapping
    {
        public TableMapping()
        {
            Columns = new List<ColumnMapping>();
            ColumnNameDict = new Dictionary<string, ColumnMapping>();
            PropertyNameDict = new Dictionary<string, ColumnMapping>();
            OtherPropertys = new List<PropertyInfo>();
        }

        public Type ObjectType { get; set; }

        public string Name { get; set; }

        public ColumnMapping ColumnPK { get; set; }

        public ColumnMapping ColumnAutoIncrement { get; set; }

        public ColumnMapping ColumnUseSeedFactory { get; set; }

        public List<ColumnMapping> Columns { get; private set; }

        public List<PropertyInfo> OtherPropertys { get; private set; }

        public Dictionary<string, ColumnMapping> ColumnNameDict { get; private set; }

        public Dictionary<string, ColumnMapping> PropertyNameDict { get; private set; }

        public bool IsContainColumn(string columnname)
        {
            return ColumnNameDict.ContainsKey(columnname);
        }

        public bool IsContainProperty(string propertyname)
        {
            return PropertyNameDict.ContainsKey(propertyname);
        }

        public ColumnMapping GetColumnMappingByColumn(string column)
        {
            if (!String.IsNullOrEmpty(column) && ColumnNameDict.ContainsKey(column))
                return ColumnNameDict[column];
            return null;
        }

        public ColumnMapping GetColumnMappingByProperty(string prop)
        {
            if (!String.IsNullOrEmpty(prop) && PropertyNameDict.ContainsKey(prop))
                return PropertyNameDict[prop];
            return null;
        }

        public void AddColumnMapping(ColumnMapping columnmapping)
        {
            if (columnmapping.IsPK)
            {
                if (ColumnPK != null)
                    throw new ObjectMappingException("table declare multi primary key");

                ColumnPK = columnmapping;
            }

            if (columnmapping.IsAutoIncrement)
            {
                if (ColumnAutoIncrement != null)
                    throw new ObjectMappingException("table declare multi autoincrementkey");

                ColumnAutoIncrement = columnmapping;
            }

            if (columnmapping.IsUseSeedFactory)
            {
                if (ColumnUseSeedFactory != null)
                    throw new ObjectMappingException("table declare multi useseedfactory");

                ColumnUseSeedFactory = columnmapping;
            }

            if ((ColumnAutoIncrement != null) && (ColumnUseSeedFactory != null))
                throw new ObjectMappingException("only use ColumnAutoIncrement or ColumnUseSeedFactory");

            this.Columns.Add(columnmapping);
            this.ColumnNameDict.Add(columnmapping.Name, columnmapping);
            this.PropertyNameDict.Add(columnmapping.PropertyName, columnmapping);
        }
    }
}
