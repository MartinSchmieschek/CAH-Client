using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Assets.Service
{
    class Game : APIBase
    {
        private JSONFromWeb playCardWebLoad;

        public void PlayCard(int cardId)
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

                Token cid = new Token()
                {
                    Name = "cardid",
                    Value = cardId.ToString()
                };


                playCardWebLoad = new JSONFromWeb("PlayCard", base.GameProperties.GameServer + @"/game/play-card", new Token[] { gid, ct,cid }, typeof(Response.PlayCard));
                playCardWebLoad.OnSuccess += new UnityAction(onPlayCardWebloadSucceded);
                playCardWebLoad.OnFail += new UnityAction(onPlayCardWebloadFailed);

                GameProperties.WebLoader.AddDownload(playCardWebLoad);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
            }
        }

        private void onPlayCardWebloadSucceded()
        {
            if (((Response.PlayCard)playCardWebLoad.Result).success)
            {
                throw new NotImplementedException();
            }
            else
            {
                base.Error = "Server rejected your request";
            }
        }

        private void onPlayCardWebloadFailed()
        {
            throw new NotImplementedException();
        }



        private JSONFromWeb getCurrentChosenCardsWebLoad;

        public void GetCurrentChosenCards()
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

                getCurrentChosenCardsWebLoad = new JSONFromWeb(" GetCurrentChosenCards", base.GameProperties.GameServer + @"/game/get-current-chosen-cards", new Token[] { gid, ct }, typeof(Response.ChosenCards));
                getCurrentChosenCardsWebLoad.OnSuccess += new UnityAction(onGetCurrentChosenCardsSucceded);
                getCurrentChosenCardsWebLoad.OnFail += new UnityAction(onGetCurrentChosenCardsFailed);

                GameProperties.WebLoader.AddDownload(getCurrentChosenCardsWebLoad);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
            }
        }

        private void onGetCurrentChosenCardsSucceded()
        {
            if (((Response.ChosenCards)getCurrentChosenCardsWebLoad.Result).success)
            {
                throw new NotImplementedException();
            }
            else
            {
                base.Error = "Server rejected your request";
            }
        }

        private void onGetCurrentChosenCardsFailed()
        {
            throw new NotImplementedException();
        }



        private JSONFromWeb chooseWinnerWebLoad;

        public void ChooseWinner(int cardid)
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

                Token cid = new Token()
                {
                    Name = "cardid",
                    Value = cardid.ToString()
                };

                chooseWinnerWebLoad = new JSONFromWeb("ChooseWinner", base.GameProperties.GameServer + @"/game/choose-winner", new Token[] { gid, ct,cid }, typeof(Response.ChooseWinner));
                chooseWinnerWebLoad.OnSuccess += new UnityAction(onChooseWinnerWebLoadSucceded);
                chooseWinnerWebLoad.OnFail += new UnityAction(onChooseWinnerWebLoadFailed);

                GameProperties.WebLoader.AddDownload(chooseWinnerWebLoad);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
            }
        }

        private void onChooseWinnerWebLoadSucceded()
        {
            if (((Response.ChooseWinner)chooseWinnerWebLoad.Result).success)
            {
                throw new NotImplementedException();
            }
            else
            {
                base.Error = "Server rejected your request";
            }
        }

        private void onChooseWinnerWebLoadFailed()
        {
            throw new NotImplementedException();
        }



        private JSONFromWeb nextRoundWebLoad;

        public void NextRound()
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

                nextRoundWebLoad = new JSONFromWeb("CheckWinner", base.GameProperties.GameServer + @"/game/check-winner", new Token[] { gid, ct }, typeof(Response.NextRound));
                nextRoundWebLoad.OnSuccess += new UnityAction(onNextRoundWebLoadSucceded);
                nextRoundWebLoad.OnFail += new UnityAction(onNextRoundWebLoadWebLoadFailed);

                GameProperties.WebLoader.AddDownload(nextRoundWebLoad);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
            }
        }

        private void onNextRoundWebLoadSucceded()
        {
            if (((Response.NextRound)nextRoundWebLoad.Result).success)
            {
                throw new NotImplementedException();
            }
            else
            {
                base.Error = "Server rejected your request";
            }
        }

        private void onNextRoundWebLoadWebLoadFailed()
        {
            throw new NotImplementedException();
        }

    }
}
