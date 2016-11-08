using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jet.Framework.ObjectMapping
{
    public class DatabaseScopeManager
    {
        private static readonly object LockedObject = new object();

        public static readonly DatabaseScopeManager Instance = new DatabaseScopeManager();

        private DatabaseScopeManager()
        {
            ThreadManagerDict = new Dictionary<Thread, ThreadManager>();
        }

        public Dictionary<Thread,ThreadManager> ThreadManagerDict {get;private set;}


        public ThreadManager CurrentThreadManager
        {
            get
            {
                lock(LockedObject)
                {
                    Thread curthread = Thread.CurrentThread;
                    if (ThreadManagerDict.ContainsKey(curthread))
                        return ThreadManagerDict[curthread];
                    else
                        return null;
                }
            }
        }

        public DatabaseSession CreateDatabaseSession(string databasename)
        {
            lock (LockedObject)
            {
                DatabaseSession newsession = new DatabaseSession(databasename);
                Thread curthread = Thread.CurrentThread;
                ThreadManager threadmanager = null;

                if (ThreadManagerDict.ContainsKey(curthread))
                    threadmanager = ThreadManagerDict[curthread];
                else
                {
                    threadmanager = new ThreadManager(curthread);
                    ThreadManagerDict.Add(curthread, threadmanager);
                }

                threadmanager.DatabaseSessionStack.Push(newsession);
                return newsession;
            }
        }

        public DatabaseSession GetCurrentDatabaseSession()
        {
            lock (LockedObject)
            {
                ThreadManager threadmanager = CurrentThreadManager;
                if ((threadmanager != null) && (threadmanager.DatabaseSessionStack.Count > 0))
                    return threadmanager.DatabaseSessionStack.Peek();

                return null;
            }
        }

        public DatabaseSession GetCurrentDatabaseSession(string databaseName)
        {
            lock (LockedObject)
            {
                ThreadManager threadmanager = CurrentThreadManager;
                if ((threadmanager != null) && (threadmanager.DatabaseSessionStack.Count > 0))
                {
                    DatabaseSession databasesession = threadmanager.DatabaseSessionStack.Peek();
                     if (databasesession.DatabaseName == databaseName)
                         return databasesession;

                    //for(int i = threadmanager.DatabaseSessionStack.Count - 1; i >= 0; i --)
                    //{
                    //    DatabaseSession databasesession = threadmanager.DatabaseSessionStack.ElementAt(i);
                    //    if (databasesession.DatabaseName == databaseName)
                    //        return databasesession;
                    //}
                }
                return null;
            }
        }

        public DatabaseSession RemoveCurrentDatabaseSession()
        {
            lock (LockedObject)
            {
                DatabaseSession databasesession = null;

                ThreadManager threadmanager = CurrentThreadManager;
                if (threadmanager != null)
                {
                    if (threadmanager.DatabaseSessionStack.Count > 0)
                        databasesession = threadmanager.DatabaseSessionStack.Pop();

                    if (threadmanager.DatabaseSessionStack.Count == 0)
                        ThreadManagerDict.Remove(threadmanager.Thread);
                }

                return databasesession;
            }
        }
    }
}
