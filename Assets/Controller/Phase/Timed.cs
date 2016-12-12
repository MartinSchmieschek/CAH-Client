using System;
using UnityEngine;
using System.Collections;

namespace Assets.Controller.Phase
{

    public class Timed : Phase
    {
        public float MaxStayOnTime = 10f;
        private float stayontime;

        public Timed()
        {
            stayontime = 0f;
            NextPhase = null;
        }

        public override IEnumerator PhaseIteration(Atom previewesPhase)
        {
            Debug.Log(String.Format("Start Phase:{0}", gameObject.name.ToString()));

            if (OnActivate != null)
                OnActivate.Invoke();

            while (IsRunning && stayontime < MaxStayOnTime)
            {
                new WaitForSeconds(Controller.UpdateTimming);
                stayontime += Time.deltaTime;
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
