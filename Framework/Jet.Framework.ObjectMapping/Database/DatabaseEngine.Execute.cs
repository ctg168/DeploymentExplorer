using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public abstract partial class DatabaseEngine
    {
        public int ExecuteNonQuery(string commandText, params SqlParameter[] paras)
        {
            DbCommand cmd = DatabaseSession.Database.GetSqlStringCommand(commandText);
            this.AddParameter(cmd, paras);
            return DatabaseSession.Database.ExecuteNonQuery(cmd);
        }

        public object ExecuteScalar(string commandText, params SqlParameter[] paras)
        {
            DbCommand cmd = DatabaseSession.Database.GetSqlStringCommand(commandText);
            this.AddParameter(cmd, paras);
            return DatabaseSession.Database.ExecuteScalar(cmd);
        }

        public IDataReader ExecuteReader(string commandText, params SqlParameter[] paras)
        {
            DbCommand cmd = DatabaseSession.Database.GetSqlStringCommand(commandText);
            this.AddParameter(cmd, paras);
            return DatabaseSession.Database.ExecuteReader(cmd);
        }

        public IDataReader ExecuteReader(DbCommand cmd)
        {
            return DatabaseSession.Database.ExecuteReader(cmd);
        }

        public DataSet ExecuteDataSet(string commandText, params SqlParameter[] paras)
        {
            DbCommand cmd = DatabaseSession.Database.GetSqlStringCommand(commandText);
            this.AddParameter(cmd, paras);
            return DatabaseSession.Database.ExecuteDataSet(cmd);
        }

        public int ExecuteNonQuery(DbCommand command)
        {
            return DatabaseSession.Database.ExecuteNonQuery(command);
        }
    }
}
