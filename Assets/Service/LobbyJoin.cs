using UnityEngine;
using System.Collections;
using Assets.Service;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace Assets.Service
{

    public class LobbyJoin : LobbyBase
    {
        private JSONFromWeb lobbyjoin;
        public UnityEvent OnJoin;

        private bool tryToJoin = false;
        private bool joined = false;

        public void JoinInLobby()
        {
            

            if (base.GameProperties.GameId != 0 && !string.IsNullOrEmpty(base.GameProperties.Token) && !tryToJoin)
            {
                joined = false;
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
                base.Error = "No GameID set";
            }

        }

        private void onJoinLobbySucceeded()
        {
            if (lobbyjoin.IsDone && ((Response.LobbyJoin)lobbyjoin.Result).success )
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
            base.Leave();
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
                            base.startGame();
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
    }
}
