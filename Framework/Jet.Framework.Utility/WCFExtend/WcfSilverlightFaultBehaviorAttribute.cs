using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public sealed class WcfFaultBehaviorAttribute : WcfBehaviorAttributeBase 
    {
        public WcfFaultBehaviorAttribute()
            : base(typeof(WcfSilverlightFaultBehavior))
        {

        }
    }
}
