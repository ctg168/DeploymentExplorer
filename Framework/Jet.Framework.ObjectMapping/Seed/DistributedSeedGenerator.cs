using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Jet.Framework.ObjectMapping
{
    public class DistributedSeedGenerator : SeedGeneratorBase
    {
        public class SeedPool
        {
            public SeedPool() { }
            public string TableName { get; internal set; }
            public int SeedValue { get; internal set; }
            public int MaxSeedValue { get; internal set; }
        }

        public DistributedSeedGenerator()
        {
            SeedPoolDict = new Dictionary<string, SeedPool>();
        }

        private Dictionary<string, SeedPool> SeedPoolDict;

        private int? GetFactorySeed(DatabaseEngine db, string tableName)
        {
            object seed = db.ExecuteScalar(string.Format("select top 1  seed from tableseed where tablename='{0}'", db.GetTableName(tableName)));
            return (seed == DBNull.Value) ? null : (int?)seed;
        }

        public override void ClearSeed(string tableName)
        {
            DatabaseEngine db = DatabaseEngineFactory.GetDatabaseEngine();
            string PoolName = string.Format("{0}_{1}", db.DatabaseSession.DatabaseName, db.GetTableName(tableName));
            lock (this)
            {
                using (DatabaseScope ds = new DatabaseScope().BeginTransaction(TransactionScopeOption.RequiresNew))
                {
                    DatabaseEngine newdb = DatabaseEngineFactory.GetNewDatabaseEngine(db.DatabaseSession.DatabaseName);
                    newdb.DatabaseSession.MappingTable = db.DatabaseSession.MappingTable;
                    newdb.ExecuteNonQuery(string.Format("delete from tableseed where tablename='{0}'", db.GetTableName(tableName)));

                    if (SeedPoolDict.ContainsKey(PoolName))
                        SeedPoolDict.Remove(PoolName);

                    ds.Complete();
                }
            }
        }

        public override int GetSeed(string tableName, string pkColumn, int SeedPoolSize = 100)
        {
            SeedPool pool = null;
            DatabaseEngine db = DatabaseEngineFactory.GetDatabaseEngine();
            lock (this)
            {
                string PoolName = string.Format("{0}_{1}", db.DatabaseSession.DatabaseName, db.GetTableName(tableName));
                if (SeedPoolDict.ContainsKey(PoolName))
                    pool = SeedPoolDict[PoolName];      //从种子缓存加载种子值
                else
                {
                    //初始化种子缓存
                    int SeedValue = 0;
                    int CurTableMaxSeed = db.GetMaxSeed(tableName, pkColumn, 0);  //从数据库表中获取当前记录中已使用的最大种子值，如果没有记录则使用默认种子值(0)

                    using (DatabaseScope ds = new DatabaseScope().BeginTransaction(TransactionScopeOption.RequiresNew))
                    {
                        DatabaseEngine newdb = DatabaseEngineFactory.GetNewDatabaseEngine(db.DatabaseSession.DatabaseName);
                        newdb.DatabaseSession.MappingTable = db.DatabaseSession.MappingTable;
                        int? FactorySeed = GetFactorySeed(newdb, tableName);
                        if (FactorySeed.HasValue)
                            SeedValue = FactorySeed.Value;
                        else
                        {
                            SeedValue = CurTableMaxSeed;
                            newdb.ExecuteNonQuery(string.Format("insert into tableseed values('{0}',{1})", db.GetTableName(tableName), SeedValue));
                        }

                        ds.Complete();
                    }

                    pool = new SeedPool();
                    pool.TableName = tableName;
                    pool.SeedValue = pool.MaxSeedValue = SeedValue;
                    SeedPoolDict[PoolName] = pool;
                }
            }

            lock (pool)
            {
                if (pool.SeedValue >= pool.MaxSeedValue)
                {
                    using (DatabaseScope ds = new DatabaseScope().BeginTransaction(TransactionScopeOption.RequiresNew))
                    {
                        DatabaseEngine newdb = DatabaseEngineFactory.GetNewDatabaseEngine(db.DatabaseSession.DatabaseName);
                        newdb.DatabaseSession.MappingTable = db.DatabaseSession.MappingTable;
                        string strSQL = string.Format("update tableseed set seed=seed+{0} where tablename='{1}'", SeedPoolSize, newdb.GetTableName(tableName));

                        int ret = newdb.ExecuteNonQuery(strSQL);
                        if (ret != 1)
                            throw new ObjectMappingException(string.Format("GetSeed Failed -> \"{0}\"", strSQL));
                        pool.MaxSeedValue = GetFactorySeed(newdb, tableName).Value;
                        ds.Complete();
                    }
                    pool.SeedValue = pool.MaxSeedValue - SeedPoolSize;
                }

                return ++pool.SeedValue;
            }
        }
    }
}
