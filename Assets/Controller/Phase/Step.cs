using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{
    public class Step : Phase
    {

        public UnityEvent OnActivate;

        public Phase NextPhase;

        public Step()
        {
            this.NextPhase = null;
        }

        public override void Tick(Phase triggerPhase)
        {
            base.Tick(triggerPhase);

            if (OnActivate != null)
                OnActivate.Invoke();
        }

        public override IEnumerator PhaseIteration(Phase previewesPhase)
        {
            Debug.Log(String.Format("Run Step:{0}", gameObject.name.ToString()));
            Controller.StartPhase(NextPhase);
            yield return null;
        }
    }
}
