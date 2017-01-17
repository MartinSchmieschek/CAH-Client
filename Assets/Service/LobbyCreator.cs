using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Service;
using UnityEngine.Events;
using Assets.Service.Response;

namespace Assets.Service
{
    public class LobbyCreator : APIBase
    {

        public string GameName { get; set;}
        public UnityEvent OnSucces;
        private JSONFromWeb data;
        bool isCreating = false;

        public void CreateLobby()
        {
            if (!string.IsNullOrEmpty(GameName) && !isCreating)
            {
                Token dat = new Token()
                {
                    Name = "clientToken",
                    Value = GameProperties.Token,
                };

                Token nam = new Token()
                {
                    Name = "gameName",
                    Value = GameName,
                };

                data = new JSONFromWeb("CreateLobby", GameProperties.GameServer + @"/lobby/create-lobby", new Token[] { dat, nam }, typeof(Response.LobbyCreate));
                data.OnFail += (new UnityAction(this.onCreationFailed));
                data.OnSuccess += (new UnityAction(this.onCreationSucceded));
                GameProperties.WebLoader.AddDownload(data);

                isCreating = true;
            }
            else
                base.Error = "No Game name assigned";

        }

        private void onCreationSucceded()
        {
            if (data.IsDone)
            {
                Response.LobbyCreate result = (Response.LobbyCreate)data.Result;
                if (result.success)
                {
                    base.GameProperties.GameId = result.game_id;

                    isCreating = false;
                    if (OnSucces != null)
                        OnSucces.Invoke();

                    
                }
                else
                {
                    Error = "Server rejected request";
                }
            }

        }

        private void onCreationFailed()
        {
            Error = "Connection failed";
            isCreating = false;
        }


    }
}

