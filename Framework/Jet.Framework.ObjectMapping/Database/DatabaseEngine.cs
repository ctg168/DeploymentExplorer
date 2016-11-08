using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public abstract partial class DatabaseEngine
    {
        public DatabaseSession DatabaseSession { get; internal set; }

        public string GetTableName(string tablename)
        {
            return DatabaseSession.GetMappingTable(tablename);
        }

        public virtual bool IsTableExsit(string tablename)
        {
            throw new ObjectMappingException("DatabaseEngine not support IsTableExsit");
        }

        public virtual void DropTable(string tablename)
        {
            throw new ObjectMappingException("DatabaseEngine not support DropTable");
        }

        public virtual int? GetMaxSeed(string tableName, string pkColumn)
        {
            object val = this.ExecuteScalar(string.Format("select max({0}) from {1}", pkColumn, this.GetTableName(tableName)));
            if (val == DBNull.Value)
                return null;
            else
                return (int?)Convert.ChangeType(val, typeof(int));
        }

        public int GetMaxSeed(string tableName, string pkColumn, int defaultValue)
        {
            int? val = GetMaxSeed(tableName, pkColumn);
            if (val.HasValue)
            {
                if (val.Value < 0)
                    return 0;
                else
                    return val.Value;
            }
            else
                return defaultValue;
        }

        public virtual string BuildParameterName(string name)
        {
            return this.GetParameterPrefix() + name;
        }

        public virtual string GetParameterPrefix()
        {
            return "@";
        }

        public void AddParameter(DbCommand command, params SqlParameter[] parameters)
        {
            foreach (SqlParameter parameter in parameters)
            {
                string name = parameter.Name;
                object val = parameter.Value;
                if (val == null)
                    val = DBNull.Value;

                DbParameter dbParameter = DatabaseSession.Database.DbProviderFactory.CreateParameter();
                dbParameter.ParameterName = name;
                if (val is DateTime)
                {
                    if ((DateTime)val == DateTime.MinValue)
                        val = DBNull.Value;
                    if (this.DatabaseSession.Database.DbProviderFactory is OleDbFactory)
                        ((OleDbParameter)dbParameter).OleDbType = OleDbType.Date;
                }
                dbParameter.Value = val;
                command.Parameters.Add(dbParameter);
            }
        }

        public int Count(Criteria criteria)
        {
            SqlParameterCollection paras = new SqlParameterCollection();
            string strSQL = this.GetCountSqlString(criteria, paras);
            var cnt = this.ExecuteScalar(strSQL, paras.ToArray());
            return (int)Convert.ChangeType(cnt, typeof(int));
        }
        public object Max(Criteria criteria, string column)
        {
            SqlParameterCollection paras = new SqlParameterCollection();
            string strSQL = this.GetMaxSqlString(criteria, column, paras);
            return this.ExecuteScalar(strSQL, paras.ToArray());
        }
    }
}
