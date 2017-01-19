using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Service.Response
{
    [Serializable]
    class NextRound : ResponseBase
    {
        public LobbyState state;
        public string winner;
    }
}
