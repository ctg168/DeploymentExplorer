using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jet.Framework.ObjectMapping
{
    public static class ORM<T>
    {
        public static Criteria<T> Criteria
        {
            get
            {
                return new Criteria<T>();
            }
        }

        public static int Create(T obj)
        {
            return DatabaseEngineFactory.GetDatabaseEngine().Create(obj);
        }

        public static int Update(T obj)
        {
            return DatabaseEngineFactory.GetDatabaseEngine().Update(obj);
        }

        public static int Delete(T obj)
        {
            return DatabaseEngineFactory.GetDatabaseEngine().Delete(obj);
        }

        public static void BulkCopy(IList objs, Dictionary<string, object> bulkCopyConfig = null)
        {
            TableMapping tableMapping = MappingService.Instance.GetTableMapping(typeof(T));
            DatabaseEngineFactory.GetDatabaseEngine().BulkCopy(tableMapping, objs, bulkCopyConfig);
        }

        public static int NewId(int SeedPoolSize = 100)
        {
            TableMapping tablemapping = MappingService.Instance.GetTableMapping(typeof(T));
            if (tablemapping.ColumnAutoIncrement != null)
                throw new ObjectMappingException(string.Format("the type of {0} not support NewId()", typeof(T).Name));

            return SeedService.Instance.Generator.GetSeed(tablemapping.Name, tablemapping.ColumnPK.Name, SeedPoolSize);
        }

        public static List<T> ExecuteQuery(string commandText)
        {
            var db = DatabaseEngineFactory.GetDatabaseEngine();
            List<T> list = new List<T>();
            using (IDataReader dr = db.ExecuteReader(commandText))
            {
                db.Load(typeof(T), list, dr);
            }
            return list;
        }
    }

    public static class ORM
    {
        public static int ExecuteNonQuery(string commandText, params SqlParameter[] paras)
        {
            return DatabaseEngineFactory.GetDatabaseEngine().ExecuteNonQuery(commandText, paras);
        }

        public static object ExecuteScalar(string commandText, params SqlParameter[] paras)
        {
            return DatabaseEngineFactory.GetDatabaseEngine().ExecuteScalar(commandText, paras);
        }

        public static int ExecuteStoreCommand(params StoreCommand[] storeCommands)
        {
            var db = DatabaseEngineFactory.GetDatabaseEngine();
            return StoreCommandHelper.ExecuteStoreCommand(db, storeCommands);
        }
    }    
}
