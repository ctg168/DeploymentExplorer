using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public abstract partial class DatabaseEngine
    {
        public virtual int Create(TableMapping table, Object obj)
        {
            //使用种子工厂--------------------------------------------------------------------------------
            if (table.ColumnPK.IsUseSeedFactory)
            {
                int tempid = (int)table.ColumnPK.PropertyInfo.GetValue(obj, null);
                if (tempid == 0)
                {
                    tempid = SeedService.Instance.Generator.GetSeed(table.Name, table.ColumnPK.Name);
                    table.ColumnPK.PropertyInfo.SetValue(obj, tempid, null);
                }
            }
            //----------------------------------------------------------------------------------------------
            SqlParameterCollection paras = new SqlParameterCollection();
            string strSQL = this.GetCreateSqlString(table, obj, paras);
            return this.ExecuteNonQuery(strSQL, paras.ToArray());
        }

        public virtual int Create(object obj)
        {
            TableMapping table = MappingService.Instance.GetTableMapping(obj.GetType());
            return Create(table, obj);
        }
    }
}
