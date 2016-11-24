using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Controller
{
    class QuitApplication : MonoBehaviour
    {
        public void DoIt()
        {
            UnityEngine.Application.Quit();
        }
    }
}
