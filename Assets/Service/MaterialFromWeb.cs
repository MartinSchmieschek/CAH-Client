using System;
using UnityEngine;

namespace Assets.Service
{
    public class MaterialFromWeb : WebData
    {
        private Material result;
        public string AssetName { get; private set; }

        public MaterialFromWeb(string name, string webadress,string assetName) : base(name, webadress)
        {
            AssetName = assetName;
        }

        public Material Result
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

        private Material getResult ()
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
                    return GameObject.Instantiate((Material)data);
                }
                throw new Exception("Asset not found in bundle");
            }    
        }
    }
}
