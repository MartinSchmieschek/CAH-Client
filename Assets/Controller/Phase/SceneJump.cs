using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Controller.Phase
{
    public class SceneJump : Atom
    {
        public string Scene;

        public override void Tick(Atom triggerPhase)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scene);
        }
    }
}
