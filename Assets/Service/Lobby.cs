using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Service
{
    [Serializable]
    public struct Lobby
    {
        public bool success;
        public LobbyInfo settings;
        public CAHPlayer[] players;
    }
}
