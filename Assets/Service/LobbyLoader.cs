using UnityEngine;
using System.Collections;
using Assets.Service;
using System.Collections.Generic;


namespace Assets.Service
{
    [RequireComponent(typeof(WebLoader))]
    public class LobbyLoader : MonoBehaviour
    {
        public string ServerAdress;
        public float AutoRefeshTime = 20;
        public List<Lobby> OpenLobbies
        {
            get
            {
                
                if (Connected)
                {
                    var tmp = new List<Lobby>();
                    foreach (var l in lobbies.lobbies)
                        if (l.state == LobbyState.STATE_INLOBBY)
                            tmp.Add(l);
                    return tmp;
                }
                return null;
            }
        }
        public bool Connected
        {
            get
            {
                if (currentLobbieLoad.IsDone && string.IsNullOrEmpty(currentLobbieLoad.Error) && lobbies.success)
                    return true;

                return false;
            }
        }

        private float refeshTimer;
        private WebLoader webLoader;
        private Lobbies lobbies;
        private JSONFromWeb currentLobbieLoad;

        public void Awake()
        {
            webLoader = GetComponent<WebLoader>();
            if (webLoader == null)
                throw new System.Exception("WebLoader cann not be located");

            getLobbies();
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
                        Debug.Log("Found "+ lobbies.lobbies.Length + "Open Lobbies");
                }
                else
                {
                    Debug.Log("Loading Lobbies:" + currentLobbieLoad.Progress + "%");
                    Debug.Log("Error:" + currentLobbieLoad.Error);
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

        public void Update()
        {
            refeshTimer += Time.deltaTime;
            if (refeshTimer > AutoRefeshTime)
            {
                Refresh();
            }
            
            processLobbyData();
        }
    }
}
