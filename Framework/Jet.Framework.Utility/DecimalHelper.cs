using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class DecimalHelper
    {
        public static decimal? Add(this decimal? val1, decimal? val2)
        {
            if (val1.HasValue && val2.HasValue)
                return val1.Value + val2.Value;
            else if (val1.HasValue && !val2.HasValue)
                return val1;
            else if (!val1.HasValue && val2.HasValue)
                return val2;
            else
                return null;
        }
    }
}
