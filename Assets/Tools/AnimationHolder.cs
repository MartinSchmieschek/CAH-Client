using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Tools
{
    [Serializable]
    public class AnimationHolderItem
    {
        public string Name;
        public Animation Animation;
        public float Delay = 0;
        
    }

    public class AnimationHolder : MonoBehaviour
    {
        public AnimationHolderItem[] Items;
        public void Awake()
        {
            foreach (var item in Items)
            {
                if (item.Animation != null)
                    item.Animation.Stop();
            }
        }

        public void Play()
        {
            foreach (var item in Items)
            {
                StartCoroutine(delayedStart(item));
            }
        }

        public void PlayReserved()
        {
            foreach (var item in Items)
            {
                StartCoroutine(delayedStart(item));
            }
        }


        private IEnumerator delayedStart(AnimationHolderItem item)
        {
            if (item.Animation != null)
            {
                yield return new WaitForSeconds(item.Delay);
                item.Animation.Play();
            }
            yield return null;
        }
    }
}
