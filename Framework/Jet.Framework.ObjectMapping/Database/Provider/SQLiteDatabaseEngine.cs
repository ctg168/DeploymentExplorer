using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public class SQLiteDatabaseEngine : GenericDatabaseEngine
    {
        public override int Create(TableMapping table, object obj)
        {
            int ret = base.Create(table, obj);
            if (table.ColumnAutoIncrement != null)
                table.ColumnAutoIncrement.SetValue(obj, GetLastInsertedIdentity());
            return ret;
        }

        /// <summary>
        /// 获得最后一次插入的自动增长值
        /// </summary>
        private int GetLastInsertedIdentity()
        {
            string strSQL = "SELECT last_insert_rowid()";
            object max = this.ExecuteScalar(strSQL);
            if (max == DBNull.Value)
                throw new ObjectMappingException("SELECT last_insert_rowid() Failed!");
            return Convert.ToInt32(max);
        }        
        
        public override string GetLoadPageSqlString(Criteria criteria, SqlParameterCollection paras)        
        {
            string strSQL = "select [COLUMNS] from [TABLE] [CONDITION] [ORDER] limit [MINROWNUM],[ROWCOUNT]";

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
            strSQL = strSQL.Replace(ConstSql.RowCount, (criteria.PageMaxRowNum - criteria.PageMinRowNum).ToString());

            return strSQL;
        }
                
        public override string GetConditionSqlString(SqlCondition condition, SqlParameterCollection paras)
        {
            string strSQL = string.Empty;
            switch (condition.Type)
            {
                case SqlConditionType.Match:
                    strSQL = string.Format("{0} like {1} ESCAPE '~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "%", "%"))));
                    break;
                case SqlConditionType.MatchPrefix:
                    strSQL = string.Format("{0} like {1} ESCAPE '~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "", "%"))));
                    break;
                case SqlConditionType.MatchSuffix:
                    strSQL = string.Format("{0} like {1} ESCAPE '~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "%", ""))));
                    break;
                case SqlConditionType.NotMatch:
                    strSQL = string.Format("{0} not like {1} ESCAPE '~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "%", "%"))));
                    break;
                case SqlConditionType.NotMatchPrefix:
                    strSQL = string.Format("{0} not like {1} ESCAPE '~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "", "%"))));
                    break;
                case SqlConditionType.NotMatchSuffix:
                    strSQL = string.Format("{0} not like {1} ESCAPE '~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "%", ""))));
                    break;
                default:
                    return base.GetConditionSqlString(condition, paras);
            }

            return strSQL;
        }

        public override string GetLoadSqlString(Criteria criteria, SqlParameterCollection paras)
        {
            string tableName = criteria.TableName;
            if (string.IsNullOrEmpty(tableName))
                tableName = this.GetTableName(criteria.TableMapping.Name);

            string strSQL = "select [COLUMNS] from [TABLE] [CONDITION] [ORDER] [TOPCOUNT]";

            strSQL = strSQL.Replace(ConstSql.Table, tableName);

            if (criteria.TakeCount > 0)
                strSQL = strSQL.Replace(ConstSql.SelectTop, " limit 0, " + criteria.TakeCount.ToString());
            else
                strSQL = strSQL.Replace(ConstSql.SelectTop, " ");

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

        /// <summary>
        /// 表是否存在于当前数据库上下文中
        /// </summary>
        public override bool IsTableExsit(string tablename)
        {
            string strSQL = string.Format("SELECT count(*) FROM sqlite_master WHERE type='table' AND [name]='{0}'", tablename);
            int ret = (int)this.ExecuteScalar(strSQL);
            return ret > 0;
        }

        /// <summary>
        /// 删除表
        /// </summary>
        public override void DropTable(string tablename)
        {
            string strSQL = string.Format("drop table {0}", tablename);
            this.ExecuteNonQuery(strSQL);
        }
    }
}
