using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Service.Response
{
    [Serializable]
    public struct Card
    {
        public int card_id;
        public string text;
        public bool is_black;
        public int blanks;
    }
}
