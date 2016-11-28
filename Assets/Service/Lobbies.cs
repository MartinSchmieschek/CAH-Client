using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Service
{
    [Serializable]
    struct Lobbies
    {
        public bool success;
        public Lobby[] lobbies;
    }
}
