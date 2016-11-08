using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jet.Framework.ObjectMapping
{
    public static class StoreCommandHelper
    {
        private const int MaxParameterCount = 1800;//最大参数数量

        private static void UnitSbCommand(StringBuilder sbCommand, string commandText, Dictionary<int, int> indexDic)
        {
            List<int> indexList = new List<int>();
            indexList.AddRange(indexDic.Keys.ToArray());
            indexList.Sort((index1, index2) => index2 - index1);
            foreach (int index in indexList)
            {
                string oldIndexStr = string.Format("{{{0}}}", index);
                string newIndexStr = string.Format("{{{0}}}", indexDic[index]);
                commandText = commandText.Replace(oldIndexStr, newIndexStr);
            }

            sbCommand.Append(commandText);

            if (!commandText.EndsWith(";"))
                sbCommand.Append(";\n");
        }

        /// <summary>
        /// 将多个StoreCommand合并成一个StoreCommand
        /// </summary>
        /// <param name="storeCommandList"></param>
        /// <returns></returns>
        private static StoreCommand UnitStoreCommand(List<StoreCommand> storeCommandList)
        {
            StoreCommand scTotal = new StoreCommand();
            StringBuilder sbCommand = new StringBuilder();
            List<object> paramterList = new List<object>();
            //Regex regex = new Regex(@"(?<=\{)(\d+)(?=\})");//"{"用的是反向正声明，"}"用的是正向声明
            Regex regex = new Regex(@"(?<=\{)(\d+)(?=\})");//"{"用的是反向正声明，"}"用的是正向声明

            int currentIndex = 0;
            Dictionary<int, int> indexDic = new Dictionary<int, int>();
            foreach (StoreCommand sc in storeCommandList)
            {
                indexDic.Clear();
                Match match = regex.Match(sc.CommandText);

                while (match.Success)
                {
                    int index = int.Parse(sc.CommandText.Substring(match.Index, match.Length));
                    if (!indexDic.ContainsKey(index))
                    {
                        indexDic.Add(index, index + currentIndex);
                    }
                    match = match.NextMatch();
                }

                UnitSbCommand(sbCommand, sc.CommandText, indexDic);
                paramterList.AddRange(sc.Parameters);
                currentIndex += indexDic.Count;
            }

            scTotal.CommandText = sbCommand.ToString();
            scTotal.Parameters = paramterList.ToArray();
            return scTotal;
        }

        public static DbCommand ConvertStoreCommandToDbCommand(DatabaseEngine db, StoreCommand storeCommand)
        {
            Regex regex = new Regex(@"\{(\d+)\}");
            string cmdText = regex.Replace(storeCommand.CommandText, db.GetParameterPrefix() + "Para$1");
            DbCommand dbCmd = db.DatabaseSession.Database.GetSqlStringCommand(cmdText);
            for (int i = 0; i < storeCommand.Parameters.Length; i++)
            {
                SqlParameter par = new SqlParameter("Para" + i.ToString(), storeCommand.Parameters[i]);
                db.AddParameter(dbCmd, par);
            }
            return dbCmd;
        }

        private static int ExecuteBatchStoreCommand(DatabaseEngine db, StoreCommand[] storeCommandList, int maxExecuteCount = 100)
        {
            int totalCount = 0;//总影响记录数
            if (maxExecuteCount <= 1)
            {
                foreach (var sc in storeCommandList)
                {
                    DbCommand dbCmd = ConvertStoreCommandToDbCommand(db, sc);
                    totalCount += db.ExecuteNonQuery(dbCmd);
                }
            }
            else
            {
                List<StoreCommand> storeCommandListTemp = new List<StoreCommand>();
                int paramerCount = 0;//参数数量
                for (int i = 0; i < storeCommandList.Length; i++)
                {
                    storeCommandListTemp.Add(storeCommandList[i]);
                    paramerCount += storeCommandList[i].Parameters.Length;

                    if (i == (storeCommandList.Length - 1) || storeCommandListTemp.Count == maxExecuteCount || paramerCount >= MaxParameterCount)
                    {
                        StoreCommand sc = StoreCommandHelper.UnitStoreCommand(storeCommandListTemp);
                        DbCommand dbCmd = ConvertStoreCommandToDbCommand(db, sc);
                        totalCount += db.ExecuteNonQuery(dbCmd);
                        storeCommandListTemp.Clear();
                        paramerCount = 0;
                    }
                }
            }

            return totalCount;
        }

        public static StoreCommand GetStoreCommand(DatabaseEngine db, string strSQL, SqlParameterCollection paras)
        {
            StoreCommand storecommand = new StoreCommand();
            storecommand.Parameters = new object[paras.Count];

            List<SqlParameter> tempparas = paras.OrderByDescending(o => o.Name.Length).ToList();

            for (int i = 0; i < tempparas.Count; i++)
            {
                strSQL = strSQL.Replace(string.Format("@{0}", tempparas[i].Name), string.Format("{{{0}}}", i));
                storecommand.Parameters[i] = tempparas[i].Value;
            }
            storecommand.CommandText = strSQL;
            return storecommand;
        }

        public static int ExecuteStoreCommand(DatabaseEngine db, string storeCommandText, params object[] parameters)
        {
            DbCommand dbCmd = ConvertStoreCommandToDbCommand(db, new StoreCommand(storeCommandText, parameters));
            return db.ExecuteNonQuery(dbCmd);
        }

        public static int ExecuteStoreCommand(DatabaseEngine db, params StoreCommand[] storeCommands)
        {
            if (storeCommands.Length == 0)
                return 0;
            else if (storeCommands.Length > 1)
                return ExecuteBatchStoreCommand(db, storeCommands);
            else
                return ExecuteStoreCommand(db, storeCommands[0].CommandText, storeCommands[0].Parameters);
        }

        public static List<T> ExecuteQuery<T>(DatabaseEngine db, StoreCommand storeCommand)
        {
            DbCommand dbCmd = ConvertStoreCommandToDbCommand(db, storeCommand);
            List<T> list = new List<T>();
            using (IDataReader dr = db.ExecuteReader(dbCmd))
            {
                db.Load(typeof(T), list, dr);
            }
            return list;
        }
    }
}
