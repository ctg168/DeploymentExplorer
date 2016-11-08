using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jet.Framework.ObjectMapping
{
    public class StoreCommand
    {
        public StoreCommand()
        {
            this.CommandText = string.Empty;
        }

        public StoreCommand(string commandText, params object[] parameters)
        {
            this.CommandText = commandText;
            this.Parameters = parameters;
        }

        public string CommandText {get;set;}

        public object[] Parameters { get; set; }

        public string SqlString
        {
            get { return this.ToString(); }
        }

        public override string ToString()
        {
            List<string> strList = new List<string>();

            foreach (object obj in this.Parameters)
            {
                if (obj == null || obj is byte[])
                {
                    strList.Add(string.Empty);
                    continue;
                }

                if (obj is string)
                    strList.Add(string.Format("'{0}'", obj));

                else
                    strList.Add(obj.ToString());
            }

            return string.Format(this.CommandText, strList.ToArray());
        }
    }
}
