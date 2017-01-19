using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Controller.Phase
{
    public class PhaseController : MonoBehaviour
    {
        private List<Phase> phases;
        public Phase CurrentPhase;
        public float UpdateTimming = 0.1f;

        public PhaseController ()
        {
            this.phases = new List<Phase>();
        }

        // Get all phases in childrens and assign this controller
        public void Awake()
        {
            Phase[] children = GetComponentsInChildren<Phase>();
            foreach(Phase gf in children) { phases.Add(gf); };
            PreparePhases();
            StartPhase(CurrentPhase);
        }

        private void PreparePhases ()
        {
            foreach (var pf in phases)
            {
                if (pf != null && pf.GetComponent<Phase>() != null)
                {
                    Debug.Log("Prepare Phase:" + pf.gameObject.name);
                    pf.Controller = this;
                }
            }
        }

        

        public void StartPhase(Phase next)
        {
            CurrentPhase.QuitPhase();

            if (next != null)
            {
                this.CurrentPhase = next;
                next.Tick(this.CurrentPhase); 
            }
            else
            {
                CurrentPhase = null;
                throw new System.NullReferenceException("Can not start phase:null");
            }
        }
    }
}