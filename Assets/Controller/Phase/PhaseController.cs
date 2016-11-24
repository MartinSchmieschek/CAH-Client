using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Controller.Phase
{
    public class PhaseController : MonoBehaviour
    {
        private List<Atom> phases;
        public Atom CurrentPhase;

        public PhaseController ()
        {
            this.phases = new List<Atom>();
        }

        // Get all phases in childrens and assign this controller
        public void Awake()
        {
            Atom[] children = GetComponentsInChildren<Atom>();
            foreach(Atom gf in children) { phases.Add(gf); };
            PreparePhases();
            StartPhase(CurrentPhase);
        }

        private void PreparePhases ()
        {
            foreach (var pf in phases)
            {
                if (pf != null && pf.GetComponent<Atom>() != null)
                {
                    Debug.Log("Prepare Phase:" + pf.gameObject.name);
                    pf.Controller = this;
                }
            }
        }

        

        public void StartPhase(Atom next)
        {
            CurrentPhase.Quit();

            if (next != null)
            {
                next.Tick(this.CurrentPhase);
                this.CurrentPhase = next;
            }
            else
            {
                throw new System.NullReferenceException("Can not start phase:null");
            }
        }
    }
}