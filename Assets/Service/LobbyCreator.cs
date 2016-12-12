using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Service;

public class LobbyCreator : MonoBehaviour {

    public WebLoader WebLoader;
    public string ServerAdress;

    public void Awake()
    {
        if (WebLoader == null)
            throw new System.Exception("WebLoader cann not be located");
    }

    public void CreateLobby()
    {
        string clientToken = "605c76cee94ff776619ae006f51d4afb";

        Token dat = new Token()
        {
            Name = "clientToken",
            Value = clientToken,
        };

        var newLobby = new JSONFromWeb("CreateLobby", ServerAdress + @"/lobby/create-lobby",dat, typeof(Lobbies));
        WebLoader.AddDownload(newLobby);
        
    }


}
