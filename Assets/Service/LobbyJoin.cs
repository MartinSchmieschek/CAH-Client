using UnityEngine;
using System.Collections;
using Assets.Service;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace Assets.Service
{
    public struct JoinLobbyResponse
    {
        public bool success;
        public string[] errors;
    }



    public class LobbyJoin : APIBase
    {
        public UnityEvent OnLeave;
        public UnityEvent OnJoin;
        public UnityEvent OnGameStart;

        public float DataUpdateTime = 30;
        private float UpdateTimer;
        private JSONFromWeb lobbyjoin;
        private JSONFromWeb lobbyWebload;
        private bool IsObserving = false;
        public Lobby CurrentLobby;
        private bool tryToJoin = false;
        private bool joined = false;

        public void JoinInLobby()
        {
            if (base.GameProperties.GameId != 0 && !string.IsNullOrEmpty(base.GameProperties.Token) && !tryToJoin)
            {
                tryToJoin = true;
                Token ct = new Token()
                {
                    Name = "clientToken",
                    Value = base.GameProperties.Token,
                };

                Token gi = new Token()
                {
                    Name = "gameId",
                    Value = base.GameProperties.GameId.ToString(),
                };

                Token[] dat = new Token[2] { ct, gi };

                lobbyjoin = new JSONFromWeb("JoinLobby", base.GameProperties.GameServer + @"/lobby/join-lobby", dat, typeof(JoinLobbyResponse));
                lobbyjoin.OnSuccess += new UnityAction(onJoinLobbySucceeded);
                lobbyjoin.OnFail += new UnityAction(onJoinLobbyFailed);

                GameProperties.WebLoader.AddDownload(lobbyjoin);
            }
            else
            {
                base.Error = "No GameID set";
            }

        }

        private void onJoinLobbySucceeded()
        {
            if (lobbyjoin.IsDone && ((JoinLobbyResponse)lobbyjoin.Result).success )
            {
                IsObserving = true;
                tryToJoin = false;
                joined = true;
                Refresh();
            }
            else
            {
                IsObserving = false;
                tryToJoin = false;
                joined = false;
                base. Error = "Server rejected you!";
            }
        }

        private void onJoinLobbyFailed()
        {
            tryToJoin = false;
            joined = false;
            IsObserving = false;
            base.Error = "Connection failed";
        }

        private void UpdateLobbyData ()
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
            if (  ((Lobby)lobbyWebload.Result).success)
            {
                CurrentLobby = (Lobby)lobbyWebload.Result;
                IsObserving = true;

                lobbyStateTest();
            }
            else
            {
                IsObserving = false;
                joined = false;
                base.Error = "Lobby webdata has not your GameId";
            }
        }

        private void onLobbyUpdateWebloadFail()
        {
            IsObserving = false;
            joined = false;
            base.Error = "Data update failed, Connection to server lost";
        }

        private void lobbyStateTest()
        {
            if (joined && IsObserving)
            {
                switch (CurrentLobby.settings.state)
                {
                    case LobbyState.STATE_INLOBBY:
                        break;
                    case LobbyState.STATE_STARTED:
                        {
                            startGame();
                        }
                        break;
                    case LobbyState.STATE_FINISHED:
                        {
                            Leave();
                        }
                        break;
                    case LobbyState.STATE_PAUSED:
                        break;
                    default:
                        break;
                }
            }
        }

        private void startGame()
        {
            if (OnGameStart != null)
                OnGameStart.Invoke();
        }

        private void Refresh()
        {
            if (!tryToJoin)
            {
                UpdateLobbyData();
                UpdateTimer = 0;
            }
        }

        public void Leave()
        {
            if (IsObserving)
            {
                IsObserving = false;
                joined = false;
            }

            if (OnLeave != null)
                OnLeave.Invoke();

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
    }
}
