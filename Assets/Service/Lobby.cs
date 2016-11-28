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
        public string game_id;
        public string create_date;
        public string last_activity;
        public string game_name;
        public LobbyState state;
        public string game_mode;
        public string target_score;
        public string kicktimer;
        public string host_user_id;

    }
}
