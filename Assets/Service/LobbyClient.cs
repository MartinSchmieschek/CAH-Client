using UnityEngine;
using System.Collections;
using Assets.Service;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace Assets.Service
{

    public class LobbyClient : LobbyBase
    {
        
        public UnityEvent OnJoin;
        private bool tryToJoin = false;

        private JSONFromWeb lobbyjoin;
        public void JoinInLobby()
        {
            if (!tryToJoin)
            {
                if (base.GameProperties.GameId != 0 && !string.IsNullOrEmpty(base.GameProperties.Token))
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

                    lobbyjoin = new JSONFromWeb("JoinLobby", base.GameProperties.GameServer + @"/lobby/join-lobby", dat, typeof(Response.LobbyJoin));
                    lobbyjoin.OnSuccess += new UnityAction(onJoinLobbySucceeded);
                    lobbyjoin.OnFail += new UnityAction(onJoinLobbyFailed);

                    GameProperties.WebLoader.AddDownload(lobbyjoin);
                }
                else
                {
                    base.Error = "No GameID or Token set";
                    base.Leave();
                }
            }
            else
            {
                Debug.Log("trying to join Lobby, a new try is not needed");
            }
        }
        private void onJoinLobbySucceeded()
        {
            if (lobbyjoin.IsDone && ((Response.LobbyJoin)lobbyjoin.Result).success )
            {
                IsObserving = true;
                tryToJoin = false;
                Refresh();
            }
            else
            {
                IsObserving = false;
                tryToJoin = false;
                base.Leave();
                base. Error = "Server rejected you!";
                
            }
        }
        private void onJoinLobbyFailed()
        {
            tryToJoin = false;
            IsObserving = false;
            base.Leave();
            base.Error = "Connection failed:" + lobbyjoin.Error;
            
        }

        public override void Refresh()
        {
            base.Refresh();
            lobbyStateTest();
        }

        private void lobbyStateTest()
        {
            if (IsObserving)
            {
                switch (CurrentLobby.settings.state)
                {
                    case LobbyState.STATE_INLOBBY:
                        break;
                    case LobbyState.STATE_STARTED:
                        {
                            IsObserving = false;
                            tryToJoin = false;
                            base.GameStarted();
                        }
                        break;
                    case LobbyState.STATE_FINISHED:
                        {
                            IsObserving = false;
                            tryToJoin = false;
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
    }
}
