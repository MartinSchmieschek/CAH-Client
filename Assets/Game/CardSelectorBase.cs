using System.Collections;
using System.Collections.Generic;
using Assets.Controller.Phase;
using UnityEngine;
using Assets.Tools;
using System;

public class CardSelectorBase : Assets.Controller.Phase.Selector {

   
    public SelectorActor ItemPrefab;
    public Phase CardSelectedPhase;
    public float ItemDistance = 1.8f;

    private SelectorActor theBlackOne;
    private List<SelectorActor> CreatedItems;
    private PositionGrid grid;

    void Start()
    {
        CreatedItems = new List<SelectorActor>();
        grid = new PositionGrid(ItemDistance, ItemDistance*1.5f, transform.position);
    }

    public void CreateBlackCard(Assets.Service.Response.Card card)
    {
        theBlackOne = CreateCard(grid.GetAtXY(5,1), transform.localScale, transform.localRotation * Quaternion.Euler(90, 180, 0), card);
    }

    public void CreateSelectorItems(Assets.Service.Response.Card[] cards)
    {
        // es sind immer 10 karten ?

        // erste reihe neben der schwarzen
        for (int i = 0; i < 4; i++)
        {
            base.AddActor(CreateCard(grid.GetAtXY(i, 1), transform.localScale, transform.localRotation * Quaternion.Euler(90, 180, 0), cards[i]));
        }

        // zweite reihe unter der schwarzen
        for (int i = 0; i < cards.Length - 4; i++)
        {
            base.AddActor(CreateCard(grid.GetAtXY(i + 4, 0 ), transform.localScale, transform.localRotation * Quaternion.Euler(90, 180, 0), cards[i]));
        }
    }

    private SelectorActor CreateCard(Vector3 pos, Vector3 scale, Quaternion rot, Assets.Service.Response.Card card)
    {
        if (ItemPrefab != null)
        {
            SelectorActor sa = Instantiate(ItemPrefab);
            sa.transform.SetParent(transform);
            sa.transform.position = pos;
            sa.transform.localScale = scale;
            sa.transform.rotation = rot;

            Step ph = sa.gameObject.GetComponent<Step>();
            ph.NextPhase = CardSelectedPhase;

            Card cardcontroller = sa.GetComponentInChildren<Card>();
            if (cardcontroller != null)
            {
                cardcontroller.CardText = card.text;
                cardcontroller.IsBlack = card.is_black;
            }
            CreatedItems.Add(sa);
            return sa;
        }

        throw new Exception("No item prefab set");
    }

}
