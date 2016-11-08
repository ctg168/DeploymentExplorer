using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Jet.Framework.ObjectMapping
{
    public class DatabaseScope : IDisposable
    {
        public DatabaseScope(string databasename = "")
        {
            DatabaseName = databasename;
            Init();
        }

        #region BeginTransaction

        public DatabaseScope BeginTransaction()
        {
            TransactionOptions to = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
            TransactionScope = new TransactionScope(TransactionScopeOption.Required, to);
            return this;
        }

        public DatabaseScope BeginTransaction(Transaction transactionToUse)
        {
            TransactionScope = new TransactionScope(transactionToUse);
            return this;
        }

        public DatabaseScope BeginTransaction(TransactionScopeOption scopeOption)
        {
            TransactionOptions to = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
            TransactionScope = new TransactionScope(scopeOption, to);
            return this;
        }

        public DatabaseScope BeginTransaction(Transaction transactionToUse, TimeSpan scopeTimeout)
        {
            TransactionScope = new TransactionScope(transactionToUse, scopeTimeout);
            return this;
        }

        public DatabaseScope BeginTransaction(TimeSpan scopeTimeout)
        {
            TransactionOptions to = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = scopeTimeout
            };
            TransactionScope = new TransactionScope(TransactionScopeOption.Required, to);
            return this;
        }

        public DatabaseScope BeginTransaction(TransactionScopeOption scopeOption, TimeSpan scopeTimeout)
        {
            TransactionOptions to = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = scopeTimeout
            };
            TransactionScope = new TransactionScope(scopeOption, to);
            return this;
        }

        public DatabaseScope BeginTransaction(TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
        {
            TransactionScope = new TransactionScope(scopeOption, transactionOptions);
            return this;
        }

        public DatabaseScope BeginTransaction(Transaction transactionToUse, TimeSpan scopeTimeout, EnterpriseServicesInteropOption interopOption)
        {
            TransactionScope = new TransactionScope(transactionToUse, scopeTimeout, interopOption);
            return this;
        }

        public DatabaseScope BeginTransaction(TransactionScopeOption scopeOption, TransactionOptions transactionOptions, EnterpriseServicesInteropOption interopOption)
        {
            TransactionScope = new TransactionScope(scopeOption, transactionOptions, interopOption);
            return this;
        }

        #endregion

        private static readonly object LockedObject = new object();

        private TransactionScope _TransactionScope;
        internal TransactionScope TransactionScope
        {
            get { return _TransactionScope; }
            set
            {
                if (_TransactionScope != null)
                    throw new ObjectMappingException("TransactionScope is opened!");

                _TransactionScope = value;
            }
        }

        public DatabaseSession DatabaseSession { get; private set; }

        public string DatabaseName { get; private set; }

        internal bool IsCreatedDatabaseSession = false;

        private void Init()
        {
            lock (LockedObject)
            {
                if (string.IsNullOrWhiteSpace(DatabaseName))
                    DatabaseSession = DatabaseScopeManager.Instance.GetCurrentDatabaseSession();
                else
                    DatabaseSession = DatabaseScopeManager.Instance.GetCurrentDatabaseSession(DatabaseName);

                if (DatabaseSession == null)
                {
                    IsCreatedDatabaseSession = true;
                    DatabaseSession = DatabaseScopeManager.Instance.CreateDatabaseSession(DatabaseName);
                }
                else
                    IsCreatedDatabaseSession = false;
            }
        }

        public void Dispose()
        {
            if (IsCreatedDatabaseSession)
                DatabaseScopeManager.Instance.RemoveCurrentDatabaseSession();

            if (TransactionScope != null)
                TransactionScope.Dispose();
        }

        public void Complete()
        {
            if (TransactionScope == null)
                throw new ObjectMappingException("TransactionScope is null");

            TransactionScope.Complete();
        }
    }
}
