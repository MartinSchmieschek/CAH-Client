using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Service.Response
{
    [Serializable]
    public class Lobbies : ResponseBase
    {
        public LobbyInfo[] lobbies;
    }
}
