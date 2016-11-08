using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public class SimpleSeedGenerator : SeedGeneratorBase
    {
        public class SeedPool
        {
            public SeedPool() { }
            public string TableName { get; internal set; }
            public int SeedValue { get; internal set; }
        }

        private Dictionary<string, SeedPool> SeedPoolDict;

        public SimpleSeedGenerator()
        {
            SeedPoolDict = new Dictionary<string, SeedPool>();
        }

        public override void ClearSeed(string tableName)
        {
            DatabaseEngine db = DatabaseEngineFactory.GetDatabaseEngine();
            string PoolName = string.Format("{0}_{1}", db.DatabaseSession.DatabaseName, db.GetTableName(tableName));
            lock (this)
            {
                if (SeedPoolDict.ContainsKey(PoolName))
                    SeedPoolDict.Remove(PoolName);
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
                    int SeedValue = db.GetMaxSeed(tableName,pkColumn,0);
                    pool = new SeedPool();
                    pool.TableName = tableName;
                    pool.SeedValue = SeedValue;
                    SeedPoolDict[PoolName] = pool;
                }
            }

            lock (pool)
            {
                return ++pool.SeedValue;
            }
        }
    }
}
