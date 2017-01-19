using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Controller.Phase
{
    public class SceneJump : Phase
    {
        public string Scene;

        public override void Tick(Phase triggerPhase)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scene);
        }
    }
}
