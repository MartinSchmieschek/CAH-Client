using System.Collections;
using System.Collections.Generic;
using Assets.Controller.Phase;
using UnityEngine;
using Assets.Tools;

public class PlayerSelectCard : CardSelectorBase {

    public GameRound GameRound;
    public override void Tick(Phase triggerPhase)
    { 
        base.CreateBlackCard(GameRound.CurrentBlackCard);
        base.CreateSelectorItems(GameRound.CurrentCards);
        base.Tick(triggerPhase);
    }


    public override void Activate()
    {
        //GameRound.PlayCard(GameRound.CurrentCards[base.SelectionIndex].card_id);
        //base.Activate();
        Debug.Log("Card with <" + GameRound.CurrentCards[base.SelectionIndex].text + "> acticated");
    }

}
