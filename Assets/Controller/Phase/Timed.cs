using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{

    public class Timed : Step
    {
        public float MaxStayOnTime = 10f;
        private float stayontime;

        public UnityEvent OnDeactivate;

        public Timed()
        {
            stayontime = 0f;
            NextPhase = null;
        }

        public override void Tick(Phase triggerPhase)
        {
            base.Tick(triggerPhase);
            if (OnActivate != null)
                OnActivate.Invoke();
        }


        public virtual void DoNextPhase()
        {
            Controller.StartPhase(NextPhase);
        }

        public override IEnumerator PhaseIteration(Phase previewesPhase)
        {
            while (IsRunning && stayontime < MaxStayOnTime)
            {
                Debug.Log(String.Format("Running Timed Phase:{0}", gameObject.name.ToString()));
                new WaitForSeconds(Controller.UpdateTimming);
                stayontime += Time.deltaTime;
                yield return null;
            }

            DoNextPhase();

            yield return null;
        }

        public override void QuitPhase()
        {
            if (OnDeactivate != null)
                OnDeactivate.Invoke();

            base.QuitPhase();
        }
    }
}