using System;
using UnityEngine;
using System.Collections;

namespace Assets.Controller.Phase
{
    public class Atom : MonoBehaviour
    {
        //public Phases ThisPhase;
        public PhaseController Controller { get; set; }

        private bool isrunning;
        public bool IsRunning { get { return isrunning; } }

        public Atom()
        {
            this.isrunning = false;
        }

        public virtual void Tick(Atom triggerPhase)
        {
            isrunning = true;
            StartCoroutine(PhaseIteration(triggerPhase));
        }

        public virtual IEnumerator PhaseIteration(Atom previewesPhase)
        {
            Debug.Log(String.Format("Start Phase:{0}", this.gameObject.name.ToString()));
            while (isrunning)
            {
                new WaitForSeconds(Controller.UpdateTimming);
                Debug.Log(String.Format("EmptyPhaseRunning!"));
                yield return null;
            }
            yield return null;
        }

        public virtual void Quit ()
        {
            isrunning = false;
        }
    }
}
