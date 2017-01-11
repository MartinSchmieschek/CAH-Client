using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{
    public class End : Atom
    {

        public UnityEvent OnActivate;
        public UnityEvent OnDeactivate;

        public override IEnumerator PhaseIteration(Atom previewesPhase)
        {
            if (OnActivate != null)
                OnActivate.Invoke();

            Debug.Log(String.Format("Start Phase:{0}", this.gameObject.name.ToString()));
            while (IsRunning)
            {
                new WaitForSeconds(Controller.UpdateTimming);
                Debug.Log(String.Format("EndPhaseRunning!"));
                yield return null;
            }

            Debug.Log(String.Format("Ending Phase:{0}", gameObject.name.ToString()));

            if (OnDeactivate != null)
                OnDeactivate.Invoke();



            yield return null;
        }
    }
}
