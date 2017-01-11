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

    public class Authenticate : APIBase
    {
        public UnityEvent OnSucces;
        public String UserName { get; set; }
        private JSONFromWeb data;


        public void DoIt()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                Token dat = new Token()
                {
                    Name = "name",
                    Value = UserName,
                };

                data = new JSONFromWeb("Authenticate", base.GameProperties.GameServer + @"/lobby/authenticate", dat, typeof(AutenticateResponse));
                data.OnSuccess += new UnityAction(connectionSucceeded);
                data.OnFail += new UnityAction(connectionFailed);
                
                GameProperties.WebLoader.AddDownload(data);
            }
            else
            {
                Error = "No User Name entered";
            }
            
        }

        private void connectionSucceeded ()
        {
            if (data.IsDone)
            {
                AutenticateResponse result = (AutenticateResponse)data.Result;
                if (result.success)
                {
                    GameProperties.UserName = UserName;
                    GameProperties.Token = result.clientToken;
                    if (OnSucces != null)
                        OnSucces.Invoke();
                }
                else
                {
                    Error = "Server rejected request";
                }
            }

        }

        private void connectionFailed()
        {
            Error = "Connection failed";
        }

    }
}
