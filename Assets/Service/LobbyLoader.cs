using UnityEngine;
using System.Collections;
using Assets.Service;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace Assets.Service
{
    public class LobbyLoader : APIBase
    {
        public float AutoRefeshTime = 30;
        public bool IsObserving { get; private set; }

        private bool isUpdating = false;

        public UnityAction OnRefreshed;

        private Response.Lobbies lobbies;
        

        public string NameFilter { get; set; }
        public List<Response.LobbyInfo> OpenLobbies
        {
            get
            {
                if (lobbies != null)
                {
                    var tmp = new List<Response.LobbyInfo>();
                    foreach (var l in lobbies.lobbies)
                        if (l.state == LobbyState.STATE_INLOBBY && l.user_count < l.max_players)
                        {
                            if (NameFilter != null && !String.IsNullOrEmpty(NameFilter))
                            {
                                if (compareName(l.game_name, NameFilter) > 0)
                                    tmp.Add(l);
                            }
                            else
                            {
                                tmp.Add(l);
                            }
                        }
                            
                    return tmp;
                }
                return null;
            }
        }

        private static int compareName(string game_name, string lobbyNameFilter)
        {
            int matchcounter = 0;
            for (int i = 0; i < lobbyNameFilter.Length; i++)
            {
                if (game_name[i] == lobbyNameFilter[i])
                    matchcounter++;
            }
            return matchcounter;
        }

        private JSONFromWeb currentLobbieLoad;
        private void getLobbies()
        {
            if (!isUpdating)
            {
                isUpdating = true;
                currentLobbieLoad = new JSONFromWeb("GetLobbies", base.GameProperties.GameServer + @"/lobby/get-lobbies", typeof(Response.Lobbies));
                currentLobbieLoad.OnSuccess += new UnityAction(currentLobbieLoadSucceded);
                currentLobbieLoad.OnFail += new UnityAction(currentLobbieLoadFailed);

                base.GameProperties.WebLoader.AddDownload(currentLobbieLoad);
            }
        }
        private void currentLobbieLoadSucceded()
        {
            if ( ((Response.Lobbies)currentLobbieLoad.Result).success )
            {
                lobbies = ((Response.Lobbies)currentLobbieLoad.Result);
                if (OnRefreshed != null)
                    OnRefreshed.Invoke();
                Debug.Log("Found " + lobbies.lobbies.Length + " Lobbies");
                isUpdating = false;
            }  
        }
        private void currentLobbieLoadFailed()
        {
            base.Error = "Connection failed:" + currentLobbieLoad.Error;
            isUpdating = false;
        }

        public void Refresh()
        {
            getLobbies();
        }
    }
}
