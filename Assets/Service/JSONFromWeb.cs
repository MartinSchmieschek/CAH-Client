using System;
using UnityEngine;

namespace Assets.Service
{
    public class JSONFromWeb : WebData
    {
        public Type JSONType { get; private set; }
        private object result;

        // Time is a workaround, caching is a big problem, with this we will get every time a none cached
        public JSONFromWeb(string name, string webadress, Type type) : base(name, webadress + "?=" + (DateTime.UtcNow.Ticks.ToString()))
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
    }
}
