using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Assets.Service;

namespace Assets.Service
{
    public class APIBase : MonoBehaviour
    {
        private List<string> Errors = new List<string>();
        public string Error {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Errors.Add(value);
                    ErrorMessage(value);
                }
            }

            private get
            {
                if (Errors.Count > 0)
                    return Errors[Errors.Count - 1];
                else
                    return null;
            }

        }
        private PeristentGameProperties gameProperties;
        public PeristentGameProperties GameProperties
        {
            get
            {
                return gameProperties;
            }
            set
            {
                gameProperties = value;
            }
        }

        public void Awake()
        {

            GameProperties = FindObjectOfType<PeristentGameProperties>();

            if (GameProperties == null)
                Error = "GameProperties can not be located";
        }

        private void ErrorMessage(string em)
        {
            // should be a errorscrren
            throw new System.Exception("CAH API Error:" + em);
        }



        //private JSONFromWeb currentGameWebLoad;
        //private UnityAction onCurrentCommunicationSucceeded;
        //public object ServerResult;

        //public void HeyServer(string name, string urlcommand,Type type, UnityAction OnSucces, UnityAction OnFail)
        //{
        //    if (GameProperties.GameId != 0 && !string.IsNullOrEmpty(GameProperties.Token))
        //    {
        //        onCurrentCommunicationSucceeded = OnSucces;

        //        Token gid = new Token()
        //        {
        //            Name = "gameId",
        //            Value = GameProperties.GameId.ToString()
        //        };

        //        Token ct = new Token()
        //        {
        //            Name = "clientToken",
        //            Value = GameProperties.Token.ToString()
        //        };

        //        currentGameWebLoad = new JSONFromWeb("name", GameProperties.GameServer + urlcommand, new Token[] { gid, ct }, typeof(Response.BlackCard));
        //        currentGameWebLoad.OnSuccess += new UnityAction(onServerCommunicationSucceded);
        //        currentGameWebLoad.OnFail += OnFail;

        //        GameProperties.WebLoader.AddDownload(currentGameWebLoad);
        //    }
        //    else
        //    {
        //        Error = "No GameID or clientToken set";
        //    }
        //}

        //private void onServerCommunicationSucceded ()
        //{
        //    if (currentGameWebLoad != null && ((Response.ResponseBase)currentGameWebLoad.Result).success)
        //    {
        //        if (onCurrentCommunicationSucceeded != null)
        //            onCurrentCommunicationSucceeded.Invoke();
        //    }
        //}
    }
}
