using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Controller
{
    public class DeactivatedOnStart : MonoBehaviour
    {
        public void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
