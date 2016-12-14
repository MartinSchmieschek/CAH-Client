using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Service;

public class LobbyCreator : MonoBehaviour {

    public PeristentGameProperties GP;

    public void Awake()
    {
        if (GP == null)
            throw new System.Exception("GameProperties can not be located");
    }

    public void CreateLobby()
    {
        Token dat = new Token()
        {
            Name = "clientToken",
            Value = GP.Token,
        };

        var newLobby = new JSONFromWeb("CreateLobby", GP.GameServer + @"/lobby/create-lobby",dat, typeof(Lobbies));
        GP.WebLoader.AddDownload(newLobby);
        
    }


}
