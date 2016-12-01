using UnityEngine;
using System.Collections;
using Assets.Service;
using Assets.Controller.Phase;
using System.Collections.Generic;
using Assets.Tools;

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

    void Awake()
    {
        lobbyLoader = GetComponent<LobbyLoader>();
        if (lobbyLoader == null)
            throw new System.Exception("No LobbyLoader assigned");
    }
	
	// Update is called once per frame
	void Update () {
	    if (lobbyLoader.Connected)
        {
            if (CreatedItems.Count == 0)
                CreateLobbyItems(1);
        }
	}

    public void CreateLobbyItems(int startid)
    {
        PositionGrid grid = new PositionGrid(ItemDistance, RowDistance, transform.position);
        var tmpLobbys = lobbyLoader.OpenLobbies;

        for (int x = 0; x < NumRowItems; x++)
        {
            for (int y = 0; y < NumRows; y++)
            {
                if (tmpLobbys.Count > (startid + x + y))
                {
                    CreateLobbyCard(grid.GetAtYZ(x, y), transform.localScale, transform.localRotation * Quaternion.Euler(-90,0,0), tmpLobbys[startid + x + y]);
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

            card.CardText = lobby.game_name;
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
