using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public abstract partial class DatabaseEngine
    {
        #region Utility
        public virtual string[] GetValuesSqlString(IList vals)
        {
            List<String> list = new List<string>();

            for (int i = 0; i < vals.Count; i++)
            {
                var val = vals[i];
                if (val == null)
                    continue;
                list.Add(this.GetValueSqlString(val));
            }

            return list.ToArray();
        }

        public virtual string GetValueSqlString(object val)
        {
            Type t = val.GetType();

            if (t == typeof(string))
                return "'" + val.ToString() + "'";
            else if (t == typeof(DateTime))
                return "'" + val.ToString() + "'";
            else if (t == typeof(bool))
                return (bool)val ? "1" : "0";
            else
                return val.ToString();
        }

        public virtual string GetLikeSqlString(object val, string StrPrefix, string StrSuffix)
        {
            return string.Format(" like '{0}' ESCAPE N'~' ", CorrectLikeConditionValue(val, StrPrefix, StrSuffix));
        }

        public virtual string CorrectFullTextConditionValue(object val)
        {
            string strval = val as string;
            if (strval == null)
                strval = string.Empty;
            strval = strval.Replace("\"", "〞");
            strval = strval.Replace("\'", "ˊ");
            return strval;
        }

        public virtual string CorrectLikeConditionValue(object val, string StrPrefix, string StrSuffix)
        {
            string strval = string.Empty;
            if (val != null)
            {
                if (val is string)
                    strval = (string)val;
                else
                    strval = val.ToString();
            }
            strval = strval.Replace("~", "~~");
            strval = strval.Replace("%", "~%");
            strval = strval.Replace("[", "~[");
            strval = strval.Replace("]", "~]");
            strval = StrPrefix + strval + StrSuffix;
            return strval;
        }
        #endregion

        #region GetSqlString Column Condition Order
        public virtual string GetOrdersSqlString(List<SqlOrder> orders)
        {
            if (orders.Count > 0)
            {
                List<String> strsql = new List<string>();
                foreach (var order in orders)
                {
                    strsql.Add(order.Column + " " + order.Type.ToString());
                }
                return string.Join(",", strsql);
            }
            else
                return string.Empty;
        }

        public virtual string GetColumnsSqlString(string pkColumn, HashSet<string> columns)
        {
            if (columns.Count > 0)
            {
                string strsql = string.Join(",", columns);
                if (!String.IsNullOrEmpty(pkColumn) && !columns.Contains(pkColumn))
                    strsql = pkColumn + "," + strsql;
                return strsql;
            }
            else
                return string.Empty;
        }


        public virtual string GetConditionsSqlString(List<SqlCondition> conditions, SqlParameterCollection paras)
        {
            if (conditions.Count > 0)
            {
                List<string> strsqls = new List<string>();
                foreach (var condition in conditions)
                {
                    String sql = this.GetConditionSqlString(condition, paras);
                    if (String.IsNullOrEmpty(sql))
                        continue;
                    strsqls.Add(sql);
                }
                return string.Join(" and ", strsqls);
            }
            else
                return string.Empty;
        }

        public virtual string GetConditionSqlString(SqlCondition condition, SqlParameterCollection paras)
        {
            string strSQL = string.Empty;
            switch (condition.Type)
            {
                case SqlConditionType.EqualTo:
                    if (condition.Value == null)
                        strSQL = condition.Column + " is null ";
                    else
                        strSQL = string.Format("{0} = {1}", condition.Column, this.BuildParameterName(paras.GenerateParameter(condition.Value)));
                    break;
                case SqlConditionType.NotEqualTo:
                    if (condition.Value == null)
                        strSQL = condition.Column + " is not null ";
                    else
                        strSQL = string.Format("{0} <> {1}", condition.Column, this.BuildParameterName(paras.GenerateParameter(condition.Value)));
                    break;
                case SqlConditionType.GreaterThan:
                    strSQL = string.Format("{0} > {1}", condition.Column, this.BuildParameterName(paras.GenerateParameter(condition.Value)));
                    break;
                case SqlConditionType.GreaterThanAndEqualTo:
                    strSQL = string.Format("{0} >= {1}", condition.Column, this.BuildParameterName(paras.GenerateParameter(condition.Value)));
                    break;
                case SqlConditionType.LessThan:
                    strSQL = string.Format("{0} < {1}", condition.Column, this.BuildParameterName(paras.GenerateParameter(condition.Value)));
                    break;
                case SqlConditionType.LessThanAndEqualTo:
                    strSQL = string.Format("{0} <= {1}", condition.Column, this.BuildParameterName(paras.GenerateParameter(condition.Value)));
                    break;
                case SqlConditionType.Match:
                    strSQL = string.Format("{0} like {1} ESCAPE N'~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "%", "%"))));
                    break;
                case SqlConditionType.MatchFullText:
                    strSQL = string.Format("CONTAINS({0},'\"{1}\"')", condition.Column, CorrectFullTextConditionValue(condition.Value));
                    break;
                case SqlConditionType.MatchPrefix:
                    strSQL = string.Format("{0} like {1} ESCAPE N'~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "", "%"))));
                    break;
                case SqlConditionType.MatchSuffix:
                    strSQL = string.Format("{0} like {1} ESCAPE N'~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "%", ""))));
                    break;
                case SqlConditionType.NotMatch:
                    strSQL = string.Format("{0} not like {1} ESCAPE N'~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "%", "%"))));
                    break;
                case SqlConditionType.NotMatchPrefix:
                    strSQL = string.Format("{0} not like {1} ESCAPE N'~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "", "%"))));
                    break;
                case SqlConditionType.NotMatchSuffix:
                    strSQL = string.Format("{0} not like {1} ESCAPE N'~'", condition.Column,
                    this.BuildParameterName(paras.GenerateParameter(CorrectLikeConditionValue(condition.Value, "%", ""))));
                    break;
                case SqlConditionType.In:
                    if (condition.Values.Count > 0)
                    {
                        string[] vals = this.GetValuesSqlString(condition.Values);
                        if (vals.Length > 0)
                            strSQL = condition.Column + " in (" + string.Join(" , ", vals) + ") ";
                        if (condition.Values.Contains(null))
                        {
                            if (string.IsNullOrEmpty(strSQL))
                                strSQL = condition.Column + " is null";
                            else
                                strSQL = "(" + strSQL + " or " + condition.Column + " is null)";
                        }
                    }
                    break;
                case SqlConditionType.NotIn:
                    strSQL = condition.Column + " not in (" + string.Join(" , ", this.GetValuesSqlString(condition.Values)) + ") ";
                    break;
                case SqlConditionType.Custom:
                    strSQL = (string)condition.Value;
                    if ((condition.Parameters != null) && (condition.Parameters.Count > 0))
                    {
                        foreach (var pitem in condition.Parameters)
                        {
                            paras.Add(pitem);
                        }
                    }
                    break;

                default:
                    break;
            }
            return strSQL;
        }
        #endregion

        #region GetSqlString Utility
        public virtual string GetCountSqlString(Criteria criteria, SqlParameterCollection paras)
        {
            string strSQL = "select count(*) from [TABLE] [CONDITION]";
            if (!string.IsNullOrEmpty(criteria.PageMaxRowCountSqlString))
                strSQL = criteria.PageMaxRowCountSqlString;

            string tableName = criteria.TableName;
            if (string.IsNullOrEmpty(tableName))
                tableName = this.GetTableName(criteria.TableMapping.Name);
            strSQL = strSQL.Replace(ConstSql.Table, tableName);

            string condition = this.GetConditionsSqlString(criteria.Conditions, paras);
            if (string.IsNullOrEmpty(condition))
                strSQL = strSQL.Replace(ConstSql.Condition, " ");
            else
                strSQL = strSQL.Replace(ConstSql.Condition, " where " + condition);

            return strSQL;
        }

        public virtual string GetMaxSqlString(Criteria criteria, string column, SqlParameterCollection paras)
        {
            string strSQL = "select max([COLUMN]) from [TABLE] [CONDITION]";
            string tableName = criteria.TableName;

            if (criteria.TableMapping != null)
            {
                if (string.IsNullOrEmpty(tableName))
                    tableName = this.GetTableName(criteria.TableMapping.Name);
                ColumnMapping columnMapping = criteria.TableMapping.GetColumnMappingByProperty(column);
                if (columnMapping != null)
                    column = columnMapping.Name;
            }

            strSQL = strSQL.Replace(ConstSql.Column, column);
            strSQL = strSQL.Replace(ConstSql.Table, tableName);

            string condition = this.GetConditionsSqlString(criteria.Conditions, paras);
            if (string.IsNullOrEmpty(condition))
                strSQL = strSQL.Replace(ConstSql.Condition, " ");
            else
                strSQL = strSQL.Replace(ConstSql.Condition, " where " + condition);

            return strSQL;
        }
        #endregion

        #region GetSqlString Query
        public virtual string GetLoadPageSqlString(Criteria criteria, SqlParameterCollection paras)
        {
            throw new ObjectMappingException("current database engine not support GetLoadPageSqlString method!");
        }

        public virtual string GetLoadSqlString(Criteria criteria, SqlParameterCollection paras)
        {
            string tableName = criteria.TableName;
            if (string.IsNullOrEmpty(tableName))
                tableName = this.GetTableName(criteria.TableMapping.Name);

            string strSQL = "select [TOPCOUNT] [COLUMNS] from [TABLE] [CONDITION] [ORDER]";

            strSQL = strSQL.Replace(ConstSql.Table, tableName);

            if (criteria.TakeCount > 0)
                strSQL = strSQL.Replace(ConstSql.SelectTop, " top " + criteria.TakeCount);
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
        #endregion

        #region GetSqlString Create
        public virtual string GetCreateSqlString(TableMapping table, object obj, SqlParameterCollection paras)
        {
            StringBuilder columnsSQL = new StringBuilder();
            StringBuilder valuesSQL = new StringBuilder();

            foreach (ColumnMapping column in table.Columns)
            {
                if (column.IsAutoIncrement || column.IsViewColumn)
                    continue;

                columnsSQL.Append(column.Name);
                columnsSQL.Append(',');

                object val = column.GetValue(obj);
                if ((val == null) && (column.ColumnType == typeof(byte[])))
                    valuesSQL.Append("NULL,");
                else
                {
                    valuesSQL.Append(this.BuildParameterName(column.Name));
                    valuesSQL.Append(',');
                    paras.Add(new SqlParameter(column.Name, val));
                }
            }

            return string.Format("insert into {0}({1}) values({2})", this.GetTableName(table.Name), columnsSQL.ToString(0, columnsSQL.Length - 1), valuesSQL.ToString(0, valuesSQL.Length - 1));
        }
        #endregion

        #region GetSqlString Delete
        public virtual string GetDeleteSqlString(Criteria criteria, SqlParameterCollection paras)
        {
            string tableName = criteria.TableName;
            if (string.IsNullOrEmpty(tableName))
                tableName = this.GetTableName(criteria.TableMapping.Name);
            string strSQL = string.Format("delete from {0} [CONDITION]", tableName);
            string condition = this.GetConditionsSqlString(criteria.Conditions, paras);
            if (string.IsNullOrEmpty(condition))
                strSQL = strSQL.Replace(ConstSql.Condition, " ");
            else
                strSQL = strSQL.Replace(ConstSql.Condition, " where " + condition);

            return strSQL;
        }
        #endregion

        #region GetSqlString Update
        public virtual string GetUpdateSqlString(Criteria criteria, SqlParameterCollection paras)
        {
            string tableName = criteria.TableName;
            if (string.IsNullOrEmpty(tableName))
                tableName = this.GetTableName(criteria.TableMapping.Name);
            string strSQL = string.Format("update {0} set ", tableName);
            foreach (SqlParameter para in criteria.UpdateParameters)
            {
                if (para.Value == null)
                    strSQL += para.Name + "=NULL ,";
                else
                {
                    strSQL += para.Name + "=" + this.BuildParameterName(para.Name) + " ,";
                    paras.Add(para);
                }
            }
            foreach (SqlParameter para in criteria.UpdateCustomParameters)
            {
                paras.Add(para);
            }
            strSQL = strSQL.Substring(0, strSQL.Length - 1);
            string condition = this.GetConditionsSqlString(criteria.Conditions, paras);
            if (!string.IsNullOrEmpty(condition))
                strSQL += " where " + condition;
            return strSQL;
        }

        #endregion
    }
}
