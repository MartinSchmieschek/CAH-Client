using UnityEngine;
using System.Collections;
using Assets.Service;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using Assets.Service.Response;

namespace Assets.Service
{
    public class LobbyHost : LobbyBase
    {
        
        private bool tryToStart = false;

        public void JoinHostedLobby()
        {
            IsObserving = true;
        }

        private JSONFromWeb startGameWebload;
        public void StartGame()
        {
            if (base.GameProperties.GameId != 0 && !string.IsNullOrEmpty(base.GameProperties.Token) && !tryToStart)
            {
                Token gid = new Token()
                {
                    Name = "gameId",
                    Value = base.GameProperties.GameId.ToString()
                };

                Token ct = new Token()
                {
                    Name = "clientToken",
                    Value = base.GameProperties.Token.ToString()
                };

                startGameWebload = new JSONFromWeb("GetLobbyState", base.GameProperties.GameServer + @"/lobby/start-game", new Token[] {gid,ct}, typeof(Response.StartGame));
                startGameWebload.OnSuccess += new UnityAction(onGameStartedWebloadSucceded);
                startGameWebload.OnFail += new UnityAction(onGameStartedWebloadFailed);

                GameProperties.WebLoader.AddDownload(startGameWebload);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
                this.Leave();
            }
        }
        private void onGameStartedWebloadSucceded ()
        {
            if (((Response.StartGame)startGameWebload.Result).success)
            {
                tryToStart = false;
                base.IsObserving = false;
                base.GameStarted();
            }
            else
            {
                tryToStart = false;
                IsObserving = false;
                base.Error = "Start Game failed, server rejected you.";
                this.Leave();
                
            }
        }
        private void onGameStartedWebloadFailed()
        {
            tryToStart = false;
            IsObserving = false;
            base.Error = "Connection failed:" + startGameWebload.Error;
            this.Leave();
        }

        
    }
}
