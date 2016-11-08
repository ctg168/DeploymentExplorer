using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public abstract class SeedGeneratorBase
    {
        public abstract void ClearSeed(string tableName);

        public abstract int GetSeed(string tableName, string pkColumn, int SeedPoolSize = 100);
    }
}
