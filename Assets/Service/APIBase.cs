using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Service;

namespace Assets.Service
{
    public class APIBase : MonoBehaviour
    {
        private List<string> Errors = new List<string>();
        public string Error {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Errors.Add(value);
                    ErrorMessage(value);
                }
            }

            private get
            {
                if (Errors.Count > 0)
                    return Errors[Errors.Count - 1];
                else
                    return null;
            }

        }
        public PeristentGameProperties GameProperties { get; private set; }
        public string Log { get; private set; }

        public void Awake()
        {

            var GameProperties = FindObjectOfType<PeristentGameProperties>();

            if (GameProperties == null)
                throw new System.Exception("GameProperties can not be located");
        }

        private void ErrorMessage(string em)
        {
            throw new System.Exception("CAH API Error:" + em);
        }
    }
}
