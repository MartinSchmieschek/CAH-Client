﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{
    public class End : Phase
    {

        public UnityEvent OnActivate;

        public override void Tick(Phase triggerPhase)
        {
            base.Tick(triggerPhase);
            if (OnActivate != null)
                OnActivate.Invoke();
        }

        public override IEnumerator PhaseIteration(Phase previewesPhase)
        {
            while (IsRunning)
            {
                Debug.Log(String.Format("Running End!:" + this.gameObject.name.ToString()));
                new WaitForSeconds(Controller.UpdateTimming);
                yield return null;
            }
            yield return null;
        }

        public override void QuitPhase()
        {
            base.QuitPhase();
        }
    }
}
