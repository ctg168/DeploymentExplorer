using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public abstract partial class DatabaseEngine
    {
        public int Update(Criteria criteria)
        {
            SqlParameterCollection paras = new SqlParameterCollection();
            string strSQL = this.GetUpdateSqlString(criteria, paras);
            return this.ExecuteNonQuery(strSQL, paras.ToArray());
        }

        public int Update(object obj)
        {
            TableMapping table = MappingService.Instance.GetTableMapping(obj.GetType());
            return Update(table, obj, new HashSet<string>());
        }

        public int Update(object obj, HashSet<string> updateprops)
        {
            TableMapping table = MappingService.Instance.GetTableMapping(obj.GetType());
            return Update(table, obj, updateprops);
        }

        public virtual int Update(TableMapping table, object obj, HashSet<string> updateprops)
        {
            Criteria criteria = new Criteria(table);
            criteria.AddEqualTo(table.ColumnPK.Name, table.ColumnPK.GetValue(obj));
            if ((updateprops != null) && (updateprops.Count > 0))
            {
                foreach (string propname in updateprops)
                {
                    ColumnMapping column = table.GetColumnMappingByProperty(propname);
                    if (column == null)
                        continue;

                    if ((column.IsPK == false) && (column.IsAutoIncrement == false))
                        criteria.AddUpdateColumn(column.Name, column.GetValue(obj));
                    else
                        throw new ObjectMappingException(propname);
                }
            }
            else
            {
                foreach (ColumnMapping column in table.Columns)
                {
                    if ((column.IsPK == false) && (column.IsAutoIncrement == false))
                    {
                        criteria.AddUpdateColumn(column.Name, column.GetValue(obj));
                    }
                }
            };
            return this.Update(criteria);
        }
    }
}
