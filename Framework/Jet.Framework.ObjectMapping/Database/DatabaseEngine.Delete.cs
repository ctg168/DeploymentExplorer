using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public abstract partial class DatabaseEngine
    {
        public int Delete(object obj)
        {
            TableMapping table = MappingService.Instance.GetTableMapping(obj.GetType());
            object id = table.ColumnPK.GetValue(obj);
            return DeleteById(table, id);
        }

        public int DeleteById(TableMapping table, object id)
        {
            Criteria criteria = new Criteria(table);
            criteria.AddEqualTo(table.ColumnPK.Name, id);
            return this.Delete(criteria);
        }

        public int Delete(Criteria criteria)
        {
            SqlParameterCollection paras = new SqlParameterCollection();
            string strSQL = this.GetDeleteSqlString(criteria, paras);
            return this.ExecuteNonQuery(strSQL, paras.ToArray());
        }
    }
}
