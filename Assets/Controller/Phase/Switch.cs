using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{
    public class Switch : Phase
    {
        public Atom[] Phases;

        public void SwitchTo(int id)
        {
            if (Phases.Length > id)
                Controller.StartPhase(Phases[id]);
            else
                Debug.Log("Index out of range!");
        }

        private void DoPhase(Atom nextPhase)
        {
            Controller.StartPhase(nextPhase);
        }
    }
}
