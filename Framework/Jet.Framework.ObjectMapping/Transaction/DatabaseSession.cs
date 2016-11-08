using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public class DatabaseSession
    {
        public DatabaseSession(string databasename)
        {
            this.DatabaseName = databasename;
            this.Database = DatabaseEngineFactory.CreateDatabase(databasename);    
        }

        public string DatabaseName { get; private set; }

        public Database Database { get; private set; }

        //-----------------------------------------------------------------------------------------------------------------------------------------
        internal Dictionary<string, string> MappingTable = new Dictionary<string, string>();

        public string GetMappingTable(string tablename)
        {
            if (MappingTable.ContainsKey(tablename))
                return MappingTable[tablename];
            else
                return tablename;
        }

        public void SetMappingTable(Type type, string tablename)
        {
            TableMapping table = MappingService.Instance.GetTableMapping(type);
            MappingTable[table.Name] = tablename;
        }

        public void SetMappingTable(string SrcTableName, string DstTableName)
        {
            MappingTable[SrcTableName] = DstTableName;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------
    }
}
