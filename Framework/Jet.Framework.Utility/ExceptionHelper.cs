using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class ExceptionHelper
    {
#if SILVERLIGHT
        public static Exception CreateException(this string err)
        {            
            return new Exception(err);
        }
#else
        public static ApplicationException CreateException(this string err)
        {
            return new ApplicationException(err);
        }
#endif
    }
}
