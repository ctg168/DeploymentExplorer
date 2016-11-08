using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public interface IObjectClone<T>
    {
        T Clone();        
    }
}
