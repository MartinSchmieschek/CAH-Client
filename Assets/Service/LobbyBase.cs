using UnityEngine;
using System.Collections;
using Assets.Service;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace Assets.Service
{
    public class LobbyBase : APIBase
    {
        public UnityEvent OnLeave;
        public UnityEvent OnGameStart;

        public float DataUpdateTime = 30;
        public float UpdateTimer;
       
        public JSONFromWeb lobbyWebload;
        public bool IsObserving = false;
        public Lobby CurrentLobby;

        public string LobbyText {
            get
            {
                if (CurrentLobby.success)
                {
      //              string players = "";
    //                foreach (var item in CurrentLobby.players.)
  //                  {
//
             //       }
                    return string.Format("GameName:{0}/n", CurrentLobby.settings.game_name)
                }

                return "not joined in lobby.";
            }
        }

        public void UpdateLobbyData()
        {
            if (base.GameProperties.GameId != 0)
            {
                Token gid = new Token()
                {
                    Name = "gameId",
                    Value = base.GameProperties.GameId.ToString(),
                };

                lobbyWebload = new JSONFromWeb("GetLobbyState", base.GameProperties.GameServer + @"/lobby/get-lobby-state", gid, typeof(Lobby));
                lobbyWebload.OnSuccess += new UnityAction(onLobbyUpdateWebloadSucceded);
                lobbyWebload.OnFail += new UnityAction(onLobbyUpdateWebloadFail);

                GameProperties.WebLoader.AddDownload(lobbyWebload);
            }
            else
            {
                base.Error = "No GameID set";
            }
        }

        private void onLobbyUpdateWebloadSucceded()
        {
            if (((Lobby)lobbyWebload.Result).success)
            {
                CurrentLobby = (Lobby)lobbyWebload.Result;
                IsObserving = true;
            }
            else
            {
                IsObserving = false;
                base.Error = "Lobby webdata has not your GameId";
            }
        }

        private void onLobbyUpdateWebloadFail()
        {
            IsObserving = false;
            base.Error = "Data update failed, Connection to server lost";
        }

        public void Refresh()
        {
            UpdateLobbyData();
            UpdateTimer = 0;
        }

        public void Update()
        {
            if (IsObserving)
            {
                UpdateTimer += Time.deltaTime;
                if (UpdateTimer > DataUpdateTime)
                {
                    Refresh();
                }
            }
        }

        public void Leave()
        {
            if (IsObserving)
            {
                IsObserving = false;
            }

            if (OnLeave != null)
                OnLeave.Invoke();

        }

        public void startGame()
        {
            if (OnGameStart != null)
                OnGameStart.Invoke();
        }



    }
}
