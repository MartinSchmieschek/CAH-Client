using UnityEngine;
using System.Collections;
using Assets.Service;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace Assets.Service
{
    [RequireComponent(typeof(WebLoader))]
    public class LobbyLoader : MonoBehaviour
    {
        public string ServerAdress;
        public float AutoRefeshTime = 30;
        public bool IsObserving { get; private set; }
        public bool Connected
        {
            get
            {
                if (!IsObserving)
                {
                    Debug.Log("There is no observing activated !");
                    return false;
                }
                if (currentLobbieLoad.IsDone && string.IsNullOrEmpty(currentLobbieLoad.Error) && lobbies.success)
                    return true;

                return false;
            }
        }

        public UnityAction OnRefreshed;

        private float refeshTimer;
        private WebLoader webLoader;

        private Lobbies lobbies;
        private JSONFromWeb currentLobbieLoad;

        public string NameFilter { get; set; }
        public List<Lobby> OpenLobbies
        {
            get
            {
                if (Connected && IsObserving)
                {
                    var tmp = new List<Lobby>();
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

        public void Awake()
        {
            webLoader = GetComponent<WebLoader>();
            if (webLoader == null)
                throw new System.Exception("WebLoader cann not be located");

            IsObserving = false;
        }

        private void getLobbies()
        {
            currentLobbieLoad = new JSONFromWeb("GetLobbies", ServerAdress + @"/lobby/get-lobbies", typeof(Lobbies));
            webLoader.AddDownload(currentLobbieLoad);
        }

        private void processLobbyData()
        {
            if (currentLobbieLoad != null)
            {
                if (currentLobbieLoad.IsDone && string.IsNullOrEmpty(currentLobbieLoad.Error))
                {
                    lobbies = (Lobbies)currentLobbieLoad.Result;
                    if (lobbies.success)
                    {
                        if (OnRefreshed != null)
                            OnRefreshed.Invoke();
                        Debug.Log("Found " + lobbies.lobbies.Length + " Lobbies");
                    }
                        
                }
                else
                {
                    Debug.Log("Loading Lobbies:" + currentLobbieLoad.Progress + "%");
                    if (String.IsNullOrEmpty(currentLobbieLoad.Error))
                    {
                        Debug.Log("Error:" + currentLobbieLoad.Error);
                    }
                    
                }
            }
            else
            {
                Debug.Log("LobbyDownload not added");
            }
        }

        public void Refresh()
        {
            getLobbies();
            refeshTimer = 0;
        }

        public void StartObserving()
        {
            if (!IsObserving)
            {
                IsObserving = true;
                Refresh();
            }
        }

        public void StopObserving()
        {
            if (IsObserving)
                IsObserving = false;
        }

        public void Update()
        {
            if (IsObserving)
            {
                refeshTimer += Time.deltaTime;
                if (refeshTimer > AutoRefeshTime)
                {
                    Refresh();
                }
            }
            processLobbyData();
        }
    }
}
