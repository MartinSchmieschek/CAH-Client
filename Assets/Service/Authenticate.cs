using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Assets.Service.Response;

namespace Assets.Service
{
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

                data = new JSONFromWeb("Authenticate", base.GameProperties.GameServer + @"/lobby/authenticate", dat, typeof(Response.Authenticate));
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
                Response.Authenticate result = (Response.Authenticate)data.Result;
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
