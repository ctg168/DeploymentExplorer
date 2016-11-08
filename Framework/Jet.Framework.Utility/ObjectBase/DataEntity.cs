using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Jet.Framework.Utility
{
    [DataContract]
    public class DataEntity : NotifyBase, IDynamicProperty
    {
        private Dictionary<string, object> _Datas = null;
        [DataMember]
        public Dictionary<string, object> Datas
        {
            get { return _Datas ?? (_Datas = new Dictionary<string, object>()); }
        }

        public void SetData(string prop, object data)
        {
            Datas[prop] = data;
        }

        public object GetData(string prop)
        {
            return Datas[prop];
        }

        public void RemoveData(string prop)
        {
            if (Datas.ContainsKey(prop))
                Datas.Remove(prop);
            else
                throw new KeyNotFoundException(prop);
        }

        public object this[string prop]
        {
            get
            {
                if (!Datas.ContainsKey(prop))
                    Datas[prop] = null;
                return Datas[prop];
            }
            set
            {
                Datas[prop] = value;
            }
        }

        public IEnumerable<string> Keys
        {
            get { return Datas.Keys; }
        }
    }
}
