using System;
using UnityEngine;

namespace Assets.Service
{
    public class JSONFromWeb : WebData
    {
        private Token[] token;
        public Type JSONType { get; private set; }
        private object result;

        // Time is a workaround, caching is a big problem, with this we will get every time a none cached
        public JSONFromWeb(string name, string webadress, Type type) : base(name, webadress + "?=" + (DateTime.UtcNow.Ticks.ToString()))
        {
            JSONType = type;
        }

        public JSONFromWeb(string name, string webadress, Token[] token, Type type) : base(name, webadress + proceedToken(token))
        {
            JSONType = type;
        }
        public JSONFromWeb(string name, string webadress, Token token, Type type) : base(name, webadress + token.GetAsFirst())
        {
            JSONType = type;
        }


        public object Result
        {
            get
            {
                if (IsDone)
                    return getResult();

                return null;
            }
        }

        public override void Dispose()
        {
            result = JsonUtility.FromJson(downloadData.text, JSONType);
            base.Dispose();
        }

        private object getResult ()
        {
            if (Disposed)
                return result;
            else
                return JsonUtility.FromJson(downloadData.text, JSONType);

        }

        private static string proceedToken(Token[] to)
        {
            bool firstDone = false;
            string tmp = "";
            foreach (var toval in to)
            {
                if (!firstDone)
                {
                    tmp += toval.GetAsFirst();
                    firstDone = true;
                }
                else
                    tmp += toval.Get();

            }
            return tmp;
        }
    }
}
