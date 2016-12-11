using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{
    public class Phase : Atom
    {
        public Atom NextPhase;
        public float UpdateTiming = 0.1f;
        public UnityEvent OnActivate;
        public UnityEvent OnDeactivate;

        public Phase ()
        {
            this.NextPhase = null;
        }

        public virtual void DoNextPhase ()
        {
           Controller.StartPhase(NextPhase);
        }

        public void DoNextPhase(Atom suggestedNextPhase)
        {
            this.NextPhase = suggestedNextPhase;
            this.DoNextPhase();
        }

        public override IEnumerator PhaseIteration(Atom previewesPhase)
        {
            Debug.Log(String.Format("Start Phase:{0}", gameObject.name.ToString()));

            if (OnActivate != null)
                OnActivate.Invoke();

            while (IsRunning)
            {
                new WaitForSeconds(UpdateTiming);
                // do your phase depending stuff here
                Debug.Log(String.Format("Running Phase:{0}", gameObject.name.ToString()));
                //
                yield return null;
            }

            Debug.Log(String.Format("Ending Phase:{0}", gameObject.name.ToString()));

            if (OnDeactivate != null)
                OnDeactivate.Invoke();

            DoNextPhase();

            yield return null;
        } 
    }
}
