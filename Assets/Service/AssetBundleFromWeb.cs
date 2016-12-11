using System;
using UnityEngine;

namespace Assets.Service
{
    public class AssetBundleFromWeb : WebData
    {
        private GameObject result;
        public string AssetName { get; private set; }

        public AssetBundleFromWeb(string name, string webadress, string assetName) : base(name, webadress)
        {
            AssetName = assetName;
        }

        public GameObject Result
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
            result = getResult();
            base.Dispose();
        }

        private GameObject getResult()
        {
            if (Disposed)
                return result;
            else
            {
                AssetBundle bundle = downloadData.assetBundle;

                var data = bundle.LoadAsset(AssetName);
                bundle.Unload(false);
                if (data != null)
                {
                    return (GameObject)data;
                }
                throw new Exception("Asset not found in bundle");
            }
        }
    }
}
