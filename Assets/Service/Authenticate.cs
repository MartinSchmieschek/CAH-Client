using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Service
{
    struct AutenticateResponse
    {
        public bool success;
        public string clientToken;
    }

    public class Authenticate : MonoBehaviour
    {
        public PeristentGameProperties GP;
        public UnityEvent OnSucces;
        public String UserName { get; set; }
        private JSONFromWeb data;

        public void Awake()
        {
            if (GP == null)
                throw new System.Exception("GameProperties can not be located");
        }

        public void DoIt()
        {
            Token dat = new Token()
            {
                Name = "name",
                Value = UserName,
            };

            data = new JSONFromWeb("Authenticate", GP.GameServer + @"/lobby/authenticate", dat, typeof(AutenticateResponse));
            data.OnSuccess += new UnityAction(downloadSucceeded);
            data.OnSuccess += new UnityAction(downloadFailed);
            GP.WebLoader.AddDownload(data);
        }

        private void downloadSucceeded ()
        {
            if (data.IsDone)
            {
                AutenticateResponse result = (AutenticateResponse)data.Result;
                if (result.success)
                {
                    GP.UserName = UserName;
                    GP.Token = result.clientToken;
                    if (OnSucces != null)
                        OnSucces.Invoke();
                }
            }

        }

        private void downloadFailed()
        {

        }

    }
}
