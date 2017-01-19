using System.Collections;
using System.Collections.Generic;
using Assets.Controller.Phase;
using UnityEngine;
using Assets.Tools;
using System;

public class JudgeSelectCard : CardSelectorBase {

    public float UpdateTiming = 4;
    public GameRound CurrentGameRound;

    public override void Tick(Phase triggerPhase)
    {
        base.Tick(triggerPhase);
        base.CreateBlackCard(CurrentGameRound.CurrentBlackCard);
    }

    public void CreatePlayerChoosenCards()
    {
        if (CurrentGameRound.CurrentChoosenCards.cards.Length > 0)
        {
            base.CreateSelectorItems(CurrentGameRound.CurrentChoosenCards.cards);
        }
    }

    // update cards choosen by players;
    public override IEnumerator PhaseIteration(Phase previewesPhase)
    {
        Debug.Log(String.Format("Start Phase:{0}", gameObject.name.ToString()));

        while (IsRunning)
        {
            Debug.Log(String.Format("Running Phase:{0}", gameObject.name.ToString()));

            CurrentGameRound.GetCurrentChosenCards();

            new WaitForSeconds(UpdateTiming);


            yield return null;
        }

        Debug.Log(String.Format("Ending Phase:{0}", gameObject.name.ToString()));
    }


}
