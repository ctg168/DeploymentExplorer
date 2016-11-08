using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jet.Framework.ObjectMapping
{
    public class ThreadManager
    {
        public ThreadManager(Thread thread)
        {
            Thread = thread;
            DatabaseSessionStack = new Stack<DatabaseSession>();
        }

        public Thread Thread {get;private set;}

        public Stack<DatabaseSession> DatabaseSessionStack {get;private set;}
    }
}
