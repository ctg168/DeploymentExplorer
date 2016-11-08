namespace Jet.Framework.DataAccess.Oracle.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;

    /// <summary>
    /// Describes a <see cref="OracleDatabase"/> instance, aggregating information from a
    /// <see cref="ConnectionStringSettings"/>.
    /// </summary>
    public class OracleDatabaseData : DatabaseData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class with a connection string and a configuration
        /// source.
        ///</summary>
        ///<param name="connectionStringSettings">The <see cref="ConnectionStringSettings"/> for the represented database.</param>
        ///<param name="configurationSource">The <see cref="IConfigurationSource"/> from which additional information can 
        /// be retrieved if necessary.</param>
        public OracleDatabaseData(ConnectionStringSettings connectionStringSettings,
                                 IConfigurationSource configurationSource)
            : base(connectionStringSettings, configurationSource)
        {
            var settings = (OracleConnectionSettings)
                           configurationSource.GetSection(OracleConnectionSettings.SectionName);

            if (settings != null)
            {
                ConnectionData = settings.OracleConnectionsData.Get(connectionStringSettings.Name);
            }
        }

        ///<summary>
        /// Gets the Oracle package mappings for the represented database.
        ///</summary>
        public IEnumerable<OraclePackageData> PackageMappings
        {
            get
            {
                return ConnectionData != null
                           ? (IEnumerable<OraclePackageData>)ConnectionData.Packages
                           : new OraclePackageData[0];
            }
        }

        private OracleConnectionData ConnectionData { get; set; }

        /// <summary>
        /// Creates a <see cref="TypeRegistration"/> instance describing the database represented by 
        /// this configuration object.
        /// </summary>
        /// <returns>A <see cref="TypeRegistration"/> instance describing a database.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations()
        {
            yield return new TypeRegistration<Database>(
                () => new OracleDatabase(
                    ConnectionString,
                    from opd in PackageMappings select (IOraclePackage)opd,
                    Container.Resolved<IDataInstrumentationProvider>(Name)))
                {
                    Name = Name,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
