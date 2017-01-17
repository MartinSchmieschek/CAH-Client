using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Service.Response
{
    public class ChosenCards : ResponseBase
    {
        public Cards[] cards;
        public bool all_chosen;
    }
}
