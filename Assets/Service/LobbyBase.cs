﻿using UnityEngine;
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

        public float DataUpdateTime = 2;
        public float UpdateTimer;
       
        public bool IsObserving = false;
        public Response.Lobby CurrentLobby;

        public string LobbyTitle {
            get
            {
                if (CurrentLobby.success)
                {
                    return string.Format("GameName:{0}/n", CurrentLobby.settings.game_name);
                }

                return "no lobby.";
            }
        }

        public JSONFromWeb lobbyWebload;
        public void UpdateLobbyData()
        {
            if (base.GameProperties.GameId != 0)
            {
                Token gid = new Token()
                {
                    Name = "gameId",
                    Value = base.GameProperties.GameId.ToString(),
                };

                lobbyWebload = new JSONFromWeb("GetLobbyState", base.GameProperties.GameServer + @"/lobby/get-lobby-state", gid, typeof(Response.Lobby));
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
            if (((Response.Lobby)lobbyWebload.Result).success)
            {
                CurrentLobby = (Response.Lobby)lobbyWebload.Result;
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
            base.Error = "Connection failed:" + lobbyWebload.Error;
        }

        public virtual void Refresh()
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
            IsObserving = false;

            base.GameProperties.GameId = 0;

            if (OnLeave != null)
                OnLeave.Invoke();

        }

        public void GameStarted()
        {
            if (OnGameStart != null)
                OnGameStart.Invoke();
        }
    }
}
