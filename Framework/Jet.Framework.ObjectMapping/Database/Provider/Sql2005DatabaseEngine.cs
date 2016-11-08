using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public class Sql2005DatabaseEngine : SqlServerDatabaseEngine
    {
        public override string GetLoadPageSqlString(Criteria criteria, SqlParameterCollection paras)
        {
            string strSQL = "select [COLUMNS] from (select row_number() over ([ORDER]) as rownum,[COLUMNS] from [TABLE] [CONDITION]) as temptable where rownum > [MINROWNUM] and rownum <= [MAXROWNUM]";
            if (!string.IsNullOrEmpty(criteria.PageGetPageDataSqlString))
                strSQL = criteria.PageGetPageDataSqlString;

            string tableName = criteria.TableName;
            if (string.IsNullOrEmpty(tableName))
                tableName = this.GetTableName(criteria.TableMapping.Name);
            strSQL = strSQL.Replace(ConstSql.Table, tableName);

            string order = this.GetOrdersSqlString(criteria.Orders);
            if (string.IsNullOrEmpty(order))
                strSQL = strSQL.Replace(ConstSql.Order, " order by " + criteria.TableMapping.ColumnPK.Name);
            else
                strSQL = strSQL.Replace(ConstSql.Order, " order by " + order);

            string pkColumn = criteria.PkColumn;
            if (!String.IsNullOrEmpty(pkColumn) && criteria.TableMapping != null)
                pkColumn = criteria.TableMapping.ColumnPK.Name;
            string columns = this.GetColumnsSqlString(pkColumn, criteria.Columns);
            if (string.IsNullOrEmpty(columns))
                strSQL = strSQL.Replace(ConstSql.Columns, " * ");
            else
                strSQL = strSQL.Replace(ConstSql.Columns, columns);

            string condition = this.GetConditionsSqlString(criteria.Conditions, paras);
            if (string.IsNullOrEmpty(condition))
                strSQL = strSQL.Replace(ConstSql.Condition, " ");
            else
                strSQL = strSQL.Replace(ConstSql.Condition, " where " + condition);

            strSQL = strSQL.Replace(ConstSql.MinRownum, criteria.PageMinRowNum.ToString());
            strSQL = strSQL.Replace(ConstSql.MaxRownum, criteria.PageMaxRowNum.ToString());

            return strSQL;
        }
    }
}
