using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Controller.Phase;
using Assets.Service;
using System;
using UnityEngine.Events;

// gehört in Service
// better if this is named Game or something like GameRound
public class GameRound : APIBase {

    public PhaseController PhaseController;
    public Phase JudgePhase;
    public Phase PlayerPhase;
    public Phase WaitForEndOfRound;
    public Phase EndOfRound;

    
    public Assets.Service.Response.Card[] CurrentCards { get; private set; }
    public Assets.Service.Response.Card CurrentBlackCard { get; private set; }

    public Assets.Service.Response.ChosenCards CurrentChoosenCards { get; private set; }
    public UnityEvent OnChoosenCardsUpdated = new UnityEvent();

    private bool cardsDrawn = false;

    // get player cards from service
    private JSONFromWeb drawCardWebLoad;
    private void drawCard()
    {
        if (!cardsDrawn)
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

                drawCardWebLoad = new JSONFromWeb("DrawCard", base.GameProperties.GameServer + @"/card/draw-card", new Token[] { gid, ct }, typeof(Assets.Service.Response.Cards));
                drawCardWebLoad.OnSuccess += new UnityAction(onDrawCardWebloadSucceded);
                drawCardWebLoad.OnFail += new UnityAction(onDrawCardWebloadFailed);

                GameProperties.WebLoader.AddDownload(drawCardWebLoad);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
            }
        }
    }
    private void onDrawCardWebloadSucceded()
    {
        if (((Assets.Service.Response.Cards)drawCardWebLoad.Result).success)
        {
            CurrentCards = ((Assets.Service.Response.Cards)drawCardWebLoad.Result).cards;
            cardsDrawn = true;
            jumptoNextPhase();
        }
        else
        {
            base.Error = "Server rejected you";
        }
    }
    private void onDrawCardWebloadFailed()
    {
        base.Error = "Connection failed:" + drawCardWebLoad.Error;
    }

    // get black card from service
    private JSONFromWeb getBlackCardWebLoad;
    private void getBlackCard()
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

            getBlackCardWebLoad = new JSONFromWeb("GetCurrentBlackCard", base.GameProperties.GameServer + @"/card/get-current-blackcard", new Token[] { gid, ct }, typeof(Assets.Service.Response.BlackCard));
            getBlackCardWebLoad.OnSuccess += new UnityAction(onGetBlackCardWebLoadSucceded);
            getBlackCardWebLoad.OnFail += new UnityAction(onGetBlackCardWebLoadFailed);

            GameProperties.WebLoader.AddDownload(getBlackCardWebLoad);
        }
        else
        {
            base.Error = "No GameID or clientToken set";
        }
    }
    private void onGetBlackCardWebLoadSucceded()
    {
        if (((Assets.Service.Response.BlackCard)getBlackCardWebLoad.Result).success)
        {
            CurrentBlackCard = ((Assets.Service.Response.BlackCard)getBlackCardWebLoad.Result).card;
        }
        else
        {
            base.Error = "Server rejected you";
        }
    }
    private void onGetBlackCardWebLoadFailed()
    {
        base.Error = "Connection failed:" + getBlackCardWebLoad.Error;
    }

    // Playing a Card
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


            playCardWebLoad = new JSONFromWeb("PlayCard", base.GameProperties.GameServer + @"/game/play-card", new Token[] { gid, ct, cid }, typeof(Assets.Service.Response.PlayCard));
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
        if (((Assets.Service.Response.PlayCard)playCardWebLoad.Result).success)
        {
            PhaseController.StartPhase(WaitForEndOfRound);
        }
        else
        {
            base.Error = "Server rejected your request";
        }
    }
    private void onPlayCardWebloadFailed()
    {
        base.Error = "Connection failed:" + playCardWebLoad.Error;
    }

    private bool updatingChoosenCards = false;
    private JSONFromWeb getCurrentChosenCardsWebLoad;
    public void GetCurrentChosenCards()
    {
        if (!updatingChoosenCards)
        {
            if (base.GameProperties.GameId != 0 && !string.IsNullOrEmpty(base.GameProperties.Token))
            {
                updatingChoosenCards = true;
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

                getCurrentChosenCardsWebLoad = new JSONFromWeb(" GetCurrentChosenCards", base.GameProperties.GameServer + @"/game/get-current-chosen-cards", new Token[] { gid, ct }, typeof(Assets.Service.Response.ChosenCards));
                getCurrentChosenCardsWebLoad.OnSuccess += new UnityAction(onGetCurrentChosenCardsSucceded);
                getCurrentChosenCardsWebLoad.OnFail += new UnityAction(onGetCurrentChosenCardsFailed);

                GameProperties.WebLoader.AddDownload(getCurrentChosenCardsWebLoad);
            }
            else
            {
                base.Error = "No GameID or clientToken set";
            }
        }
        else
        {
            Debug.Log("updatingChoosenCards canceld, o other update is not finished!");
        }
    }
    private void onGetCurrentChosenCardsSucceded()
    {
        if (((Assets.Service.Response.ChosenCards)getCurrentChosenCardsWebLoad.Result).success)
        {
            CurrentChoosenCards = (Assets.Service.Response.ChosenCards)getCurrentChosenCardsWebLoad.Result;
            if (OnChoosenCardsUpdated != null)
                OnChoosenCardsUpdated.Invoke();
            updatingChoosenCards = false;
        }
        else
        {
            updatingChoosenCards = false;
            base.Error = "Server rejected your request";  
        }
    }
    private void onGetCurrentChosenCardsFailed()
    {
        updatingChoosenCards = false;
        base.Error = "Connection failed:" + getCurrentChosenCardsWebLoad.Error;
    }

    public void StartRound ()
    {
        getBlackCard(); // BlackCard.Card ist Null wenn man der Judge ist, muss mit den Schnitzmeiern geklärt werden
        drawCard();
    }

    private void jumptoNextPhase ()
    {
        if (CurrentCards.Length > 1)
        {
            PhaseController.StartPhase(PlayerPhase);
        }
        else
        {
            // workaround zum getBlack Card.card=null problem
            CurrentBlackCard = CurrentCards[0];
            CurrentCards = null;

            PhaseController.StartPhase(JudgePhase);
        }
    }

    

}
