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
        public int game_id;
        public DateTime create_date;
        public DateTime last_activity;
        public string game_name;
        public LobbyState state;
        public int game_mode;
        public int target_score;
        public int kicktimer;
        public int host_user_id;
        public int user_count;
        public int max_players;
        public Category[] categories;
    }
}
