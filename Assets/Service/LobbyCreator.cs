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
        
        bool isCreating = false;

        private JSONFromWeb createLobbyWebload;
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

                createLobbyWebload = new JSONFromWeb("CreateLobby", GameProperties.GameServer + @"/lobby/create-lobby", new Token[] { dat, nam }, typeof(Response.LobbyCreate));
                createLobbyWebload.OnFail += (new UnityAction(this.onCreationFailed));
                createLobbyWebload.OnSuccess += (new UnityAction(this.onCreationSucceded));
                GameProperties.WebLoader.AddDownload(createLobbyWebload);

                isCreating = true;
            }
            else
                base.Error = "No Game name assigned";

        }
        private void onCreationSucceded()
        {
            if (createLobbyWebload.IsDone)
            {
                Response.LobbyCreate result = (Response.LobbyCreate)createLobbyWebload.Result;
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
            base.Error = "Connection failed:" + createLobbyWebload.Error;
            isCreating = false;
        }


    }
}

