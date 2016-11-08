using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public class OracleDatabaseEngine : GenericDatabaseEngine
    {
        public override string GetParameterPrefix()
        {
            return ":";
        }

        public override string GetLoadSqlString(Criteria criteria, SqlParameterCollection paras)
        {
            string tableName = criteria.TableName;
            if (string.IsNullOrEmpty(tableName))
                tableName = this.GetTableName(criteria.TableMapping.Name);

            //string strSQL = "select [COLUMNS] from [TABLE] [CONDITION] [ORDER] [TOPCOUNT]";
            string strSQL = string.Empty;
            if (criteria.Orders.Count > 0)
                strSQL = "select [COLUMNS] from(select [COLUMNS] from [TABLE] [CONDITION] [ORDER]) where [TOPCOUNT]";
            else
                strSQL = "select [COLUMNS] from [TABLE] [CONDITION] [TOPCOUNT]";

            strSQL = strSQL.Replace(ConstSql.Table, tableName);

            if (criteria.TakeCount > 0)
            {
                if (criteria.Orders.Count > 0)
                    strSQL = strSQL.Replace(ConstSql.SelectTop, " rownum<=" + criteria.TakeCount.ToString());
                else
                {
                    if(criteria.Conditions.Count > 0)
                        strSQL = strSQL.Replace(ConstSql.SelectTop, " and rownum<=" + criteria.TakeCount.ToString());
                    else
                        strSQL = strSQL.Replace(ConstSql.SelectTop, " where rownum<=" + criteria.TakeCount.ToString());
                }
            }
            else
            {
                strSQL = strSQL.Replace(ConstSql.SelectTop, " ");
            }

            string condition = this.GetConditionsSqlString(criteria.Conditions, paras);
            if (condition == string.Empty)
                strSQL = strSQL.Replace(ConstSql.Condition, " ");
            else
                strSQL = strSQL.Replace(ConstSql.Condition, " where " + condition);

            string pkColumn = criteria.PkColumn;
            if (!String.IsNullOrEmpty(pkColumn) && criteria.TableMapping != null)
                pkColumn = criteria.TableMapping.ColumnPK.Name;
            string columns = this.GetColumnsSqlString(pkColumn, criteria.Columns);
            if (columns == string.Empty)
                strSQL = strSQL.Replace(ConstSql.Columns, " * ");
            else
                strSQL = strSQL.Replace(ConstSql.Columns, columns);

            string order = this.GetOrdersSqlString(criteria.Orders);
            if (order == string.Empty)
                strSQL = strSQL.Replace(ConstSql.Order, " ");
            else
                strSQL = strSQL.Replace(ConstSql.Order, " order by " + order);

            return strSQL;
        }
    }
}
