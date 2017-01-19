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
    public Phase JoinLobbyPhase;
    public float UpdateTimming = 15f;

    private List<SelectorActor> CreatedItems = new List<SelectorActor>();
    private List<LobbyInfo> onScreenShownLobbys = new List<LobbyInfo>();

    private int currentStartId = 0;

    void Start()
    {
        lobbyLoader = GetComponent<LobbyLoader>();
        if (lobbyLoader == null)
            throw new System.Exception("No LobbyLoader assigned");
    }

    // erste initialisierung
    private UnityAction ItemInitalisierung;

    private void initItems()
    {
        lobbyLoader.Refresh();
        ItemInitalisierung = new UnityAction(finishInit);
        lobbyLoader.OnRefreshed += ItemInitalisierung;
    }

    private void finishInit()
    {
        lobbyLoader.OnRefreshed -= ItemInitalisierung;
        CleanCreatedItems();
        CreateLobbyItems();
    }

    // Controller Toggle
    public override void Tick(Phase triggerPhase)
    {
        initItems();
        base.Tick(triggerPhase);
    }

    // Item List Handling
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
        onScreenShownLobbys = new List<LobbyInfo>();
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

            Step ph = sa.gameObject.GetComponent<Step>();
            ph.NextPhase = JoinLobbyPhase;

            Card card = sa.GetComponentInChildren<Card>();
            if (card != null)
            {
                //\nLast Activity:{4}minutes ago
                //(DateTime.Now - lobby.last_activity).TotalMinutes) // Anzeige ist nicht korrekt, lobby.last_activity überprüfen

                card.CardText = String.Format("Name:\n{0}\n\nPlayers:{1}/{2}\nTarget Score:{3}",
                lobby.game_name, lobby.user_count, lobby.max_players, lobby.target_score);
            }

            CreatedItems.Add(sa);
            base.AddActor(sa);
            onScreenShownLobbys.Add(lobby);

        }
    }

    public override void Activate()
    {
        lobbyLoader.GameProperties.GameId = onScreenShownLobbys[base.SelectionIndex].game_id;
        base.Activate();
        
    }

    public override void QuitPhase()
    {
        CleanCreatedItems();
        base.QuitPhase();
    }

    // Updating
    public override IEnumerator PhaseIteration(Phase previewesPhase)
    {
        Debug.Log(String.Format("Start Phase:{0}", gameObject.name.ToString()));

        while (IsRunning)
        {
            Debug.Log(String.Format("Running Phase:{0}", gameObject.name.ToString()));

            lobbyLoader.Refresh();

            new WaitForSeconds(UpdateTimming);

            yield return null;
        }

        Debug.Log(String.Format("Ending Phase:{0}", gameObject.name.ToString()));
    }

}
