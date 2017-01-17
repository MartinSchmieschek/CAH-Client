using UnityEngine;
using System.Collections;
using Assets.Service;
using Assets.Controller.Phase;
using System.Collections.Generic;
using Assets.Tools;
using UnityEngine.Events;
using System;
using Assets.Service.Response;

[RequireComponent(typeof(LobbyLoader))]
public class LobbySelector : Selector {

    private LobbyLoader lobbyLoader;
    public SelectorActor ItemPrefab;
    public int NumRowItems = 5;
    public float ItemDistance = 1.2f;
    public int NumRows = 2;
    public float RowDistance = 2f;
    public Atom JoinLobbyPhase;

    private List<SelectorActor> CreatedItems = new List<SelectorActor>();
    private List<LobbyInfo> shownLobbys = new List<LobbyInfo>();

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
            if (currentStartId < lobbyLoader.OpenLobbies.Count)
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
            foreach (var item in CreatedItems)
                item.Kill();
        }

        CreatedItems = new List<SelectorActor>();
        base.SelectableItems = new SelectorItem[]{ };
        shownLobbys = new List<LobbyInfo>();
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
                    CreateLobbyCard(grid.GetAtXY(x, y), transform.localScale, transform.localRotation * Quaternion.Euler(90,180,0), tmpLobbys[currentStartId + x + y]);
                }
            }
        }
    }

    private void CreateLobbyCard(Vector3 pos, Vector3 scale, Quaternion rot,LobbyInfo lobby)
    {
        if (ItemPrefab != null)
        {
            SelectorActor sa = Instantiate(ItemPrefab);
            sa.transform.SetParent(transform);
            sa.transform.position = pos;
            sa.transform.localScale = scale;
            sa.transform.rotation = rot;

            Phase ph = sa.gameObject.GetComponent<Phase>();
            ph.NextPhase = JoinLobbyPhase;

            Card card = sa.GetComponentInChildren<Card>();
            if (card != null)
            {
                card.CardText = String.Format("Name:\n{0}\n\nPlayers:{1}/{2}\nTarget Score:{3}\nLast Activity:{4}minutes ago",
                lobby.game_name, lobby.user_count, lobby.max_players, lobby.target_score, (DateTime.Now - lobby.last_activity).TotalMinutes);
            }

            CreatedItems.Add(sa);
            base.AddActor(sa);
            shownLobbys.Add(lobby);

        }
    }

    public override void Activate()
    {
        base.Activate();
        lobbyLoader.StopObserving();
        lobbyLoader.GameProperties.GameId = shownLobbys[base.SelectionIndex].game_id;
    }

}
