using System;
using UnityEngine.Events;

namespace Assets.Service
{
    class Card : APIBase
    {
        private JSONFromWeb drawCardWebLoad;

        public void DrawCard()
        {
            if (base.GameProperties.GameId != 0 && !string.IsNullOrEmpty(base.GameProperties.Token))
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

                drawCardWebLoad = new JSONFromWeb("GetLobbyState", base.GameProperties.GameServer + @"/card/draw-card", new Token[] { gid, ct }, typeof(Response.Cards));
                drawCardWebLoad.OnSuccess += new UnityAction(onDrawCardWebloadSucceded);
                drawCardWebLoad.OnFail += new UnityAction(onDrawCardWebloadFailed);

                GameProperties.WebLoader.AddDownload(drawCardWebLoad);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
            }
        }

        private void onDrawCardWebloadSucceded()
        {
            if (((Response.Cards)drawCardWebLoad.Result).success)
            {
                throw new NotImplementedException();
            }
            else
            {
                base.Error = "Lobby webdata has not your GameId";
            }
        }

        private void onDrawCardWebloadFailed()
        {
            throw new NotImplementedException();
        }


        private JSONFromWeb getCurrentCardsWebLoad;

        public void GetCurrentCards()
        {
            if (base.GameProperties.GameId != 0 && !string.IsNullOrEmpty(base.GameProperties.Token))
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

                getCurrentCardsWebLoad = new JSONFromWeb("GetCurrentCards", base.GameProperties.GameServer + @"/card/get-current-cards", new Token[] { gid, ct }, typeof(Response.Cards));
                getCurrentCardsWebLoad.OnSuccess += new UnityAction(this.onGetCurrentCardsWebloadSucceded);
                getCurrentCardsWebLoad.OnFail += new UnityAction(onGetCurrentCardsWebloadFailed);

                GameProperties.WebLoader.AddDownload(getCurrentCardsWebLoad);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
            }
        }

        private void onGetCurrentCardsWebloadSucceded()
        {
            if (((Response.Cards)drawCardWebLoad.Result).success)
            {
                throw new NotImplementedException();
            }
            else
            {
                base.Error = "Lobby webdata has not your GameId";
            }
        }

        private void onGetCurrentCardsWebloadFailed()
        {
            throw new NotImplementedException();
        }


        private JSONFromWeb getCurrentBlackCardWebLoad;

        public void GetCurrentBlackCard()
        {
            if (base.GameProperties.GameId != 0 && !string.IsNullOrEmpty(base.GameProperties.Token))
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

                getCurrentBlackCardWebLoad = new JSONFromWeb("GetCurrentBlackCard", base.GameProperties.GameServer + @"/card/get-current-blackcard", new Token[] { gid, ct }, typeof(Response.BlackCard));
                getCurrentBlackCardWebLoad.OnSuccess += new UnityAction(onGetCurrentBlackCardWebLoadSucceded);
                getCurrentBlackCardWebLoad.OnFail += new UnityAction(onGetCurrentBlackCardWebLoadFailed);

                GameProperties.WebLoader.AddDownload(getCurrentCardsWebLoad);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
            }
        }

        private void onGetCurrentBlackCardWebLoadSucceded()
        {
            if (((Response.Cards)drawCardWebLoad.Result).success)
            {
                throw new NotImplementedException();
            }
            else
            {
                base.Error = "Lobby webdata has not your GameId";
            }
        }

        private void onGetCurrentBlackCardWebLoadFailed()
        {
            throw new NotImplementedException();
        }
    }


}
