using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Controller.Phase
{
    [RequireComponent(typeof(Collider))]
    public class SelectorActor : MonoBehaviour
    {

        public Animator animator;

        private void getAnimator()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        // Use this for initialization
        void Start()
        {
            getAnimator();
            animator.SetTrigger("spawn");
        }

        public void OnDestroy()
        {
            animator.SetTrigger("kill");
        }

        public void Activate()
        {
            getAnimator();
            animator.SetTrigger("activated");
        }
        public void Select()
        {
            getAnimator();
            animator.SetBool("selected",true);
        }

        public void Deselect()
        {
            getAnimator();
            animator.SetBool("selected", false);
        }
    }

}
