using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;

namespace Assets.Service
{
    public class WebLoader : MonoBehaviour
    {
        public float DownloadStatusUpdateTimmer = 0.05f;
        public float QueueUpdateTimer = 0.1f;
        private bool isLoading = false;
        public bool IsLoading { get { return this.isLoading; } }
        private int historySize = 100;

        // dataload
        private List<WebData> downloads = new List<WebData>();
        private List<WebData> failed = new List<WebData>();
        private List<WebData> done = new List<WebData>();

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
        }

        private void proceedDownload()
        {
            if (downloads != null && downloads.Count > 0)
            {
                WebData nextDload = downloads.FirstOrDefault<WebData>();

                // remove Failed Loads
                if (!string.IsNullOrEmpty(nextDload.Error))
                {
                    failed.Add(nextDload);
                    downloads.Remove(nextDload);

                    if (nextDload.OnFail != null)
                        nextDload.OnFail.Invoke();

                    return;
                }

                // remove done dloads
                if (nextDload.IsDone && string.IsNullOrEmpty(nextDload.Error))
                {
                    done.Add(nextDload);
                    downloads.Remove(nextDload);

                    if (nextDload.OnSuccess != null)
                        nextDload.OnSuccess.Invoke();

                    return;
                }

                // not Started
                if (!nextDload.IsDone && nextDload.downloadData == null)
                {
                    if (nextDload.OnStart != null)
                        nextDload.OnStart.Invoke();

                    StartCoroutine(LoadWebData(nextDload));
                    return;
                }
            }
        }

        IEnumerator LoadWebData(WebData wData)
        {
            while (!wData.IsDone && string.IsNullOrEmpty(wData.Error))
            {
                wData.Load();
                while (string.IsNullOrEmpty(wData.Error) && !wData.IsDone)
                {
                    new WaitForSeconds(DownloadStatusUpdateTimmer);
                    yield return wData;
                }

                if (wData.IsDone && string.IsNullOrEmpty(wData.Error))
                {
                    wData.Dispose();
                    yield return wData;
                }
                else
                {
                    Debug.Log(wData.Error);
                    yield return wData;
                }
            }

            yield return wData;

        }

        IEnumerator ProceedQueue()
        {
            isLoading = true;
            while (downloads.Count > 0)
            {
                proceedDownload();
                yield return new WaitForSeconds(QueueUpdateTimer);

                CleanHistory();
            }
            isLoading = false;
            yield return null;
        }

        public void AddDownload(WebData dload)
        {
            downloads.Add(dload);

            if (!isLoading)
                StartCoroutine(ProceedQueue());
        }

        public void Awake()
        {
            Resources.UnloadUnusedAssets();
        }

        private void CleanHistory()
        {
            if (failed.Count > historySize)
                failed.RemoveRange(0, (int)(historySize / 2));

            if (done.Count > historySize)
                done.RemoveRange(0, (int)(historySize / 2));
        }
    }
}
