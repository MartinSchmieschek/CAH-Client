using Assets.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PeristentGameProperties : MonoBehaviour
{
    public string GameServer;
    public WebLoader WebLoader;
    public string UserName;
    public string Token;
    public int GameId;

    public void SetGameId (int value)
    {
        GameId = value;
    }
}

