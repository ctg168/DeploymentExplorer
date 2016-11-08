using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;

namespace Jet.Framework.ObjectMapping
{
    public enum EnumDatabaseEngineType
    {
        GenericDatabase,
        Sql2005,
        Sql2008,
        Sql2012,
        SQLite,
        Oracle,
    }

    public static class DatabaseEngineFactory
    {
        private static Dictionary<string, EnumDatabaseEngineType> DBTypeDict = new Dictionary<string, EnumDatabaseEngineType>();
        private static object LockedObject = new object();

        private static DatabaseProviderFactory databaseProviderFactory = new DatabaseProviderFactory();

        public static Database CreateDatabase(string databasename)
        {
            return databaseProviderFactory.Create(databasename);
        }

        public static DatabaseEngine GetNewDatabaseEngine(string databasename)
        {
            lock (LockedObject)
            {
                DatabaseEngine engine = null;
                EnumDatabaseEngineType enginetype = GetDatabaseEngineType(databasename);
                switch (enginetype)
                {
                    case EnumDatabaseEngineType.Sql2005: engine = new Sql2005DatabaseEngine(); break;
                    case EnumDatabaseEngineType.Sql2008: engine = new Sql2008DatabaseEngine(); break;
                    case EnumDatabaseEngineType.Sql2012: engine = new Sql2012DatabaseEngine(); break;
                    case EnumDatabaseEngineType.SQLite: engine = new SQLiteDatabaseEngine(); break;
                    case EnumDatabaseEngineType.Oracle: engine = new OracleDatabaseEngine(); break;
                    case EnumDatabaseEngineType.GenericDatabase: engine = new GenericDatabaseEngine(); break;
                }
                DatabaseSession session = new DatabaseSession(databasename);
                engine.DatabaseSession = session;

                return engine;
            }
        }

        public static DatabaseEngine GetDatabaseEngine(string databasename = "")
        {
            lock (LockedObject)
            {
                DatabaseSession session = null;
                if (string.IsNullOrWhiteSpace(databasename))
                    session = DatabaseScopeManager.Instance.GetCurrentDatabaseSession();
                else
                    session = DatabaseScopeManager.Instance.GetCurrentDatabaseSession(databasename);
                DatabaseEngine engine = null;
                EnumDatabaseEngineType enginetype = GetDatabaseEngineType(session != null ? session.DatabaseName : databasename);
                switch (enginetype)
                {
                    case EnumDatabaseEngineType.Sql2005: engine = new Sql2005DatabaseEngine(); break;
                    case EnumDatabaseEngineType.Sql2008: engine = new Sql2008DatabaseEngine(); break;
                    case EnumDatabaseEngineType.Sql2012: engine = new Sql2012DatabaseEngine(); break;
                    case EnumDatabaseEngineType.SQLite: engine = new SQLiteDatabaseEngine(); break;
                    case EnumDatabaseEngineType.Oracle: engine = new OracleDatabaseEngine(); break;
                    case EnumDatabaseEngineType.GenericDatabase: engine = new GenericDatabaseEngine(); break;
                }

                if (engine != null)
                {
                    if (session == null)
                        session = new DatabaseSession(databasename);
                    engine.DatabaseSession = session;
                }
                else
                    throw new ObjectMappingException("not find database engine");

                return engine;
            }
        }

        internal static EnumDatabaseEngineType GetDatabaseEngineType(string databasename)
        {
            lock (LockedObject)
            {
                EnumDatabaseEngineType databasetype = EnumDatabaseEngineType.GenericDatabase;
                if (DBTypeDict.ContainsKey(databasename))
                    return DBTypeDict[databasename];
                else
                {
                    Database database = CreateDatabase(databasename);
                    if (database is SqlDatabase)
                    {
                        string version = (string)database.ExecuteScalar(CommandType.Text, "select @@version");

                        if (version.IndexOf("Microsoft SQL Server 2005") == 0)
                        {
                            databasetype = EnumDatabaseEngineType.Sql2005;
                            DBTypeDict.Add(databasename, databasetype);
                        }
                        else if (version.IndexOf("Microsoft SQL Server 2008") == 0)
                        {
                            databasetype = EnumDatabaseEngineType.Sql2008;
                            DBTypeDict.Add(databasename, databasetype);
                        }
                        else if (version.IndexOf("Microsoft SQL Server 2012") == 0)
                        {
                            databasetype = EnumDatabaseEngineType.Sql2012;
                            DBTypeDict.Add(databasename, databasetype);
                        }
                        else
                            throw new ObjectMappingException("not support sqlserver version -> " + version);
                    }
                    else if (database is OracleDatabase)
                    {
                        databasetype = EnumDatabaseEngineType.Oracle;
                        DBTypeDict.Add(databasename, databasetype);
                    }
                    else if (database is GenericDatabase)
                    {
                        if (database.DbProviderFactory is SQLiteFactory)
                        {
                            databasetype = EnumDatabaseEngineType.SQLite;
                            DBTypeDict.Add(databasename, databasetype);
                        }
                        else
                        {
                            databasetype = EnumDatabaseEngineType.GenericDatabase;
                            DBTypeDict.Add(databasename, databasetype);
                        }
                    }
                }
                return databasetype;
            }
        }
    }
}
