using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Service
{
    public class Token
    {
        public string Name;
        public string Value;

        public string GetAsFirst()
        {
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Value))
            {
                return String.Format("?{0}={1}", Name, Value);
            }
            else
            {
                return "";
            }
        }

        public string Get()
        {
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Value))
            {
                return String.Format("&{0}={1}", Name, Value);
            }
            else
            {
                return "";
            }
        }
    }
}
