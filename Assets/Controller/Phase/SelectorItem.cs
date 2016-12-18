using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{
    [Serializable]
    public class SelectorItem
    {
        public Atom Phase = null;
        public UnityEvent OnSelected;
        public UnityEvent OnDeselected;
        public UnityEvent OnActivated;
        public Collider MouseCollider;


    }
}
