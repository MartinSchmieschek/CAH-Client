using UnityEngine;

namespace Assets.Service
{
    public abstract class WebData
    {
        public string Name { get; private set; }
        public string WebAdress { get; private set; }
        private int numReconnections = 0;
        public int NumReconnections
        {
            get
            {
                return numReconnections;
            }
        }

        public WWW downloadData { get; private set; }
        public bool Disposed { get; private set; }

        private bool disposedIsDone = false;
        public bool IsDone
        {
            get
            {
                if (Disposed)
                    return disposedIsDone;

                if (downloadData != null)
                    return downloadData.isDone;

                return false;
            }
        }
        private float disposedProgress = 0.0f;
        public float Progress
        {
            get
            {
                if (Disposed)
                    return disposedProgress * 100.0f;

                if (downloadData != null)
                    return downloadData.progress * 100.0f;

                return 0f;
            }
        }
        private string disposedError;
        public string Error
        {
            get
            {
                if (Disposed)
                    return disposedError;

                if (downloadData != null)
                    return downloadData.error;

                return null;
            }
        }

        public WebData(string name, string webadress)
        {
            Name = name;
            WebAdress = webadress;
        }

        public virtual void Load()
        {
            if (downloadData != null)
            {
                downloadData.Dispose();
                downloadData = null;
                numReconnections++;
            }
            Disposed = false;
            downloadData = new WWW(WebAdress);
        }

        public virtual void Dispose()
        {
            if (downloadData != null)
            {
                disposedIsDone = downloadData.isDone;
                disposedProgress = downloadData.progress;
                disposedError = downloadData.error;

                downloadData.Dispose();
                downloadData = null;
                Disposed = true;
            }
        }
    }
}
