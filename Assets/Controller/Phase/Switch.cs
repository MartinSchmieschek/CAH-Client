using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{
    public class Switch : Atom
    {
        public float UpdateTiming = 0.1f;
        public UnityEvent OnActivate;
        public UnityEvent OnDeactivate;

        public Switch()
        {

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

            yield return null;
        } 
    }
}
