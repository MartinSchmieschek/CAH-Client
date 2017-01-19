using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Service.Response
{
    [Serializable]
    public class LobbyJoin : ResponseBase
    {
        public string[] errors;
    }
}
