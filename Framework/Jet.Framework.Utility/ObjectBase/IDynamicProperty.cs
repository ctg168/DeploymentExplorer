using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public interface IDynamicProperty
    {
        Dictionary<string, object> Datas
        {
            get;
        }

        void SetData(string prop, object data);

        object GetData(string prop);

        void RemoveData(string prop);

        object this[string prop]
        {
            get;
            set;
        }

        IEnumerable<string> Keys
        {
            get;
        }
    }
}
