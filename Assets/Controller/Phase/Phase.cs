using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{
    public class Phase : Atom
    {
        public Atom NextPhase;
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

        public override IEnumerator PhaseIteration(Atom previewesPhase)
        {
            Debug.Log(String.Format("Start Phase:{0}", gameObject.name.ToString()));

            if (OnActivate != null)
                OnActivate.Invoke();

            while (IsRunning)
            {
                new WaitForSeconds(Controller.UpdateTimming);
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
