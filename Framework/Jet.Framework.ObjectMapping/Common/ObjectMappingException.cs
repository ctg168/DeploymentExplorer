using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public class ObjectMappingException : Exception
    {
        public ObjectMappingException(string message)
            : base(message)
        {

        }
    }
}
