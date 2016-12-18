using UnityEngine;
using System.Collections;
using Assets.Service;
using Assets.Controller.Phase;
using System.Collections.Generic;
using Assets.Tools;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(LobbyLoader))]
public class LobbySelector : Selector {

    private LobbyLoader lobbyLoader;
    public Card ItemPrefab;
    public int NumRowItems = 5;
    public float ItemDistance = 1.2f;
    public int NumRows = 2;
    public float RowDistance = 2f;

    private List<SelectorItem> CreatedItems = new List<SelectorItem>();
    private List<Card> CreatedVisuals = new List<Card>();

    private int currentStartId = 0;

    void Start()
    {
        lobbyLoader = GetComponent<LobbyLoader>();
        if (lobbyLoader == null)
            throw new System.Exception("No LobbyLoader assigned");
        //lobbyLoader.OnRefresh += new UnityAction(UpdateLobbyData);
    }

    public void ScrollListBack()
    {
        if (IsRunning)
            if (currentStartId > 0)
            {
                currentStartId--;
                UpdateLobbyItems();
            }
            
    }
    public void ScrollListForward()
    {
        if (IsRunning)
            if (currentStartId + (NumRowItems* NumRows) < lobbyLoader.OpenLobbies.Count)
            {
                currentStartId++;
                UpdateLobbyItems();
            }
    }

    // Update is called once per frame
    void Update () {
	    if (lobbyLoader.Connected)
        {
            if (CreatedItems.Count == 0)
                CreateLobbyItems();
        }
	}

    public void UpdateLobbyItems()
    {
        CleanCreatedItems();
        CreateLobbyItems();
    }

    private void CleanCreatedItems()
    {
        if (CreatedItems.Count > 0)
        {

        }

        if (CreatedVisuals.Count > 0)
        {
            foreach (var item in CreatedVisuals)
                Destroy(item);
        }
    }

    public override IEnumerator PhaseIteration(Atom previewesPhase)
    {
        Debug.Log(String.Format("Start Phase:{0}", gameObject.name.ToString()));

        while (IsRunning)
        {
            Debug.Log(String.Format("Running Phase:{0}", gameObject.name.ToString()));

            new WaitForSeconds(Controller.UpdateTimming);

            // Update lobby list silent

            yield return null;
        }

        Debug.Log(String.Format("Ending Phase:{0}", gameObject.name.ToString()));
    }

    private void CreateLobbyItems()
    {
        PositionGrid grid = new PositionGrid(ItemDistance, RowDistance, transform.position);
        var tmpLobbys = lobbyLoader.OpenLobbies;

        for (int x = 0; x < NumRowItems; x++)
        {
            for (int y = 0; y < NumRows; y++)
            {
                if (tmpLobbys.Count > (currentStartId + x + y))
                {
                    CreateLobbyCard(grid.GetAtXY(x, y), transform.localScale, transform.localRotation * Quaternion.Euler(-90,0,0), tmpLobbys[currentStartId + x + y]);
                }
            }
        }
    }

    private void CreateLobbyCard(Vector3 pos, Vector3 scale, Quaternion rot,Lobby lobby)
    {
        if (ItemPrefab != null)
        {
            Card card = Instantiate(ItemPrefab);
            card.transform.SetParent(transform);

            card.CardText = String.Format("Name:\n{0}\n\nPlayers:{1}/{2}\nTarget Score:{3}\nLast Activity:{4}minutes ago", 
                lobby.game_name,lobby.user_count,lobby.max_players,lobby.target_score,(DateTime.Now - lobby.last_activity).TotalMinutes);
            card.transform.position = pos;
            card.transform.localScale = scale;
            card.transform.rotation = rot;
            CreatedVisuals.Add(card);

            SceneJump ph = card.gameObject.AddComponent<SceneJump>();

            SelectorItem item = new SelectorItem();
            item.MouseCollider = card.GetComponent<Collider>();
            item.Phase = ph;
            CreatedItems.Add(item);

        }
    }

}
