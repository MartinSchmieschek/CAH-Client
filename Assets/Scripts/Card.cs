using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{

    private string cardText = "no text";
    public string CardText
    {
        get
        {
            return cardText;
        }
        set
        {
            cardText = value;
            setTextToCard();

        }
    }
    public float TextSize
    {
        get
        {
            if (textMesh != null)
            {
                return textMesh.characterSize;
            }
            throw new System.Exception("Card has no TextMesh");
        }
        set
        {
            if (textMesh != null)
            {
                textMesh.characterSize = value;
            }
            throw new System.Exception("Card has no TextMesh");
        }
    }

    public float TextVisibility = 100;
    public TextMesh textMesh;
    public MeshRenderer cardMeshRenderer;
    private bool isBlack;
    public bool IsBlack
    {
        get
        {
            return isBlack;
        }
        set
        {
            isBlack = value;
            this.Morph();
        }
    }
    private Animator animator;
    public int MaxCharsOnCard = 20;
    public Material WhiteCardMaterial;
    public Material BlackCardMaterial;
    public Animator Animator
    {
        get
        {
            return animator;
        }
    }

    void Awake()
    {

    }

    public void Update()
    {
        if (TextVisibility < 100f)
        {
            setTextToCard();
        }
    }

    private void setTextToCard()
    {
        
        string final = "";
        string currLine = "";

        if (TextVisibility < 0)
            TextVisibility = 0;
        if (TextVisibility > 100)
            TextVisibility = 100f;

        //Text wrapping
        string[] words = CardText.Split(' ');

        foreach (string word in words)
        {
            int l = currLine.Length + word.Length;

            if (l < MaxCharsOnCard)
            {
                currLine += word + " ";
            }
            else if (l > MaxCharsOnCard)
            {
                currLine += "\n";
                final += currLine;
                currLine = "";
                currLine += word + " ";
            }
        }
        final += currLine;

        //Text fade in
        //string tmp = "";
        //for (int i = 0; i < (int)(final.Length * (TextVisibility / 100)); i++)
        //{
        //    tmp += final[i];
        //}
        //final = tmp;

        textMesh.text = final;
    }

    private void Morph()
    {
        if (isBlack && BlackCardMaterial != null)
        {
            cardMeshRenderer.material = BlackCardMaterial;
            textMesh.color = new Color(255, 255, 255, 255);
        }
        else
        {
            cardMeshRenderer.material = WhiteCardMaterial;
            textMesh.color = new Color(0, 0, 0, 255);
        }
    }


}
