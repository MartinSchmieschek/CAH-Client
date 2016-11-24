using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestShit : MonoBehaviour {

    public GameObject CardPrefab;
    private List<Card> cards;

    private Vector3 spawnpos;

	// Use this for initialization
	void Start () {
        cards = new List<Card>();
        spawnpos = new Vector3(0,0,0);

    }

    private bool keydown = false;
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Space) && !keydown)
        {
            keydown = true;
            Card c = spawnCard(spawnpos);
            c.CardText = "fkjhsdkfhdskjfhsdkh kjdfsghsdkjfhsdkjfhsd dskjhfksdhfksdhfksd lkhgdsklghdsklgh hfskdhfkdshfd hkdsfhdskhfdsk hsdkfhdskfhdskdf hdfkjhsdk fdskfhkhfkds dskfjh dskfhds jhhfds kdjfh dsk";
            cards.Add(c);
            spawnpos.x += 1.2f;
        }

        if (Input.GetKey(KeyCode.Backspace) && !keydown)
        {
            keydown = true;
            if (cards.Count > 0)
            {
                Destroy(cards[cards.Count - 1]);
                cards.RemoveAt(cards.Count - 1);
                spawnpos.x -= 1.2f;
            }
            
        }

        if (!Input.anyKey)
            keydown = false;
	

    }

    private Card spawnCard (Vector3 pos)
    {
        GameObject tmp = Instantiate(CardPrefab);

        if (tmp != null)
        {
            tmp.transform.position = pos;
        }
        else
            throw new System.Exception("Cardspawn failed!");

        Card card = tmp.GetComponent<Card>();

        if (card != null)
        {
            return card;  
        }
            
        else
        {
            Destroy(tmp);
            throw new System.Exception("Cardspawn failed!");
        }
            
    }

}
