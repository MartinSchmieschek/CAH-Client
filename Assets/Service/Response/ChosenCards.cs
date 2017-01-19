using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Service.Response
{
    [Serializable]
    public class ChosenCards : ResponseBase
    {
        public Card[] cards;
        public bool all_chosen;
    }
}
