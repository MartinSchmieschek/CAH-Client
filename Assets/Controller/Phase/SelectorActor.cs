using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Controller.Phase
{
    [RequireComponent(typeof(Phase))]
    public class SelectorActor : MonoBehaviour
    {

        public Animator animator;
        public float DestroyDelay = 2f;

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

        public void Kill()
        {
            StartCoroutine(delayedDestroy());
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

        private IEnumerator delayedDestroy ()
        {
            animator.SetTrigger("kill");
            yield return new WaitForSeconds(DestroyDelay);
            Destroy(gameObject);
        }
    }

}
