using System;
using UnityEngine;
using System.Collections;

namespace Assets.Controller.Phase
{
    /// <summary>
    /// handles the controller interaction
    /// </summary>
    public class Phase : MonoBehaviour
    {
        public PhaseController Controller { get; set; }

        /// <summary>
        /// Checks in Controller if this phase is running
        /// </summary>
        public bool IsRunning {
            get
            {
                if (Controller != null)
                {
                    if (((Phase)Controller.CurrentPhase).Equals(this))
                    {
                        return true;
                    }
                }

                return false;


            } }


        /// <summary>
        /// Starts Atom, will be called by controller
        /// </summary>
        /// <param name="triggerPhase"></param>
        public virtual void Tick(Phase triggerPhase)
        {
            Debug.Log(String.Format("Start Phase:{0}", this.gameObject.name.ToString()));
       //     isrunning = true;
            StartCoroutine(PhaseIteration(triggerPhase));
        }

        /// <summary>
        /// Parraler updating for Game stuff
        /// </summary>
        /// <param name="previewesPhase"></param>
        /// <returns></returns>
        public virtual IEnumerator PhaseIteration(Phase previewesPhase)
        {
            while (IsRunning)
            {
                Debug.Log(String.Format("Running Atom!"));
                new WaitForSeconds(Controller.UpdateTimming);
                yield return null;
            }
            yield return null;
        }

        /// <summary>
        /// Ends the Phase, will be called by the controller
        /// </summary>
        public virtual void QuitPhase ()
        {
            Debug.Log(String.Format("Ending Phase:{0}", gameObject.name.ToString()));
          //  isrunning = false;
        }
    }
}
