using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;

namespace Assets.Service
{
    class WebLoader : MonoBehaviour
    {
        public int maxReconnects = 4;
        public float ReConnectDelay = 1f;
        public float UpdateTimmer = 0.25f;

        // dataload
        private List<WebData> downloads;
        private List<WebData> failed;
        private List<WebData> done;

        public string StatusText
        {
            get
            {
                string tmp = "";

                foreach (var d in done)
                {
                    tmp += genStatusText(d);
                }

                foreach (var fail in failed)
                {
                    tmp += genStatusText(fail);
                }

                foreach (var dl in downloads)
                {
                    tmp += genStatusText(dl);
                }

                return tmp;
            }
        }
        private string genStatusText(WebData data)
        {
            string tmp = "";
            if (data != null)
            {
                tmp += data.Name + ":";

                if (!string.IsNullOrEmpty(data.Error))
                    tmp += data.Error.ToString();
                else
                    if (data.Progress < 100f)
                {
                    tmp += (data.Progress.ToString() + "%");
                }
                else
                    tmp += "done";

            }
            else
            {
                tmp += "null Data";
            }
            tmp += "\n";
            return tmp;




            return tmp;
        }

        private void proceedDownload()
        {
            if (downloads != null && downloads.Count > 0)
            {
                WebData nextDload = downloads.FirstOrDefault<WebData>();

                // remove Failed Loads
                if (nextDload.IsDone && !string.IsNullOrEmpty(nextDload.Error))
                {
                    failed.Add(nextDload);
                    downloads.Remove(nextDload);
                    return;
                }

                // remove done dloads
                if (nextDload.IsDone && string.IsNullOrEmpty(nextDload.Error))
                {
                    done.Add(nextDload);
                    downloads.Remove(nextDload);
                    return;
                }

                // not Started
                if (!nextDload.IsDone && nextDload.downloadData == null)
                {
                    StartCoroutine(LoadWebData(nextDload));
                    return;
                }
            }
        }

        IEnumerator LoadWebData(WebData wData)
        {
            while (!wData.IsDone && wData.NumReconnections < maxReconnects)
            {
                wData.Load();
                while (string.IsNullOrEmpty(wData.Error) && !wData.IsDone)
                {
                    new WaitForSeconds(UpdateTimmer);
                }

                if (wData.IsDone && string.IsNullOrEmpty(wData.Error))
                {
                    wData.Dispose();
                    yield break;
                }
                else
                {
                    Debug.Log(wData.Error);
                    new WaitForSeconds(ReConnectDelay);
                }
            }
        }

        public void AddDownload(WebData dload)
        {
            downloads.Add(dload);
        }

        public void Awake()
        {
            downloads = new List<WebData>();
            failed = new List<WebData>();
            done = new List<WebData>();
        }

        public void Update()
        {
            proceedDownload();
        }
    }
}
