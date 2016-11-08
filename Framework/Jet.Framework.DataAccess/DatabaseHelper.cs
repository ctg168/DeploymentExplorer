using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Jet.Framework.DataAccess
{
    public class DatabaseHelper
    {
        private IConfigurationSource configSource = null;
        //private DatabaseProviderFactory factory = null;
        private Database dataBase = null;

        public DatabaseHelper(string configurationFilepath = null)
        {
            if (configurationFilepath != null)
            {
                configSource = new FileConfigurationSource(configurationFilepath);
                dataBase = EnterpriseLibraryContainer.CreateDefaultContainer(configSource).GetInstance<Database>();
            }
            else
            {
                dataBase = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            }
        }

        private DbCommand GetCommand(string commandText, int commandTimeout = 300)
        {
            DbCommand command = dataBase.DbProviderFactory.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;
            command.CommandTimeout = commandTimeout;
            return command;
        }

        public string GetAppSetting(string key)
        {
            if (configSource == null)
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }
            else
            {
                return configSource.GetSection("appSettings").CurrentConfiguration.AppSettings.Settings[key].Value;
            }
        }

        public IDataReader ExecuteReader(string commandText, int commandTimeout = 300)
        {
            DbCommand command = this.GetCommand(commandText, commandTimeout);
            return dataBase.ExecuteReader(command);
        }

        public DataTable ExecureDataTable(string commandText, int commandTimeout = 300)
        {
            DbCommand command = this.GetCommand(commandText, commandTimeout);

            DataSet ds = dataBase.ExecuteDataSet(command);
            return ds.Tables[0];
        }
    }
}
