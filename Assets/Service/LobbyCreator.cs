using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Service;
using UnityEngine.Events;

namespace Assets.Service
{
    public struct CreateLobbyResponse
    {
        public bool success;
        public int game_id;
    }

    public class LobbyCreator : APIBase
    {

        public string GameName = "";
        public UnityEvent OnSucces;
        private JSONFromWeb data;

        public void CreateLobby()
        {
            if (!string.IsNullOrEmpty(GameName))
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

                data = new JSONFromWeb("CreateLobby", GameProperties.GameServer + @"/lobby/create-lobby", new Token[] { dat, nam }, typeof(CreateLobbyResponse));
                data.OnFail += (new UnityAction(this.connectionFailed));
                data.OnSuccess += (new UnityAction(this.connectionFailed));
                GameProperties.WebLoader.AddDownload(data);
            }
            else
                base.Error = "No Game name assigned";

        }

        private void connectionSucceeded()
        {
            if (data.IsDone)
            {
                CreateLobbyResponse result = (CreateLobbyResponse)data.Result;
                if (result.success)
                {
                    base.GameProperties.GameId = result.game_id;
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

