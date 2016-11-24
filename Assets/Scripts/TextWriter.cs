using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TextWriter : MonoBehaviour
{

    public Card CardPrefab;
    public string Text = "no text";
    public float LetterTimer = 0.1f;
    public float LetterDistance = 50f;
    private Vector3 currentPos;
    private GameObject previewCube;

    private List<Card> writtenLetters = new List<Card>();

    [ContextMenu("Preview")]
    void Preview()
    {
        previewCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        previewCube.transform.position = gameObject.transform.position;
        previewCube.transform.rotation = gameObject.transform.rotation;
        previewCube.transform.localScale = new Vector3((normalisedScale() * LetterDistance) * Text.Length - 1, 0.1f, 65);
        previewCube.transform.SetParent(gameObject.transform);
        previewCube.name = "PreviewCube";

        
    }

    [ContextMenu("Clear")]
    void Clear()
    {
        if (previewCube != null)
            Destroy(previewCube);
    }

    public void Awake()
    {
        Clear();
    }

    private float normalisedScale()
    {
        return (gameObject.transform.localScale.x + gameObject.transform.localScale.y + gameObject.transform.localScale.z) / 3;
    }

    public void OnDestroy()
    {
        clearText();
    }

    private void clearText()
    {
        if (writtenLetters != null)
        {
            foreach (var item in writtenLetters)
                Destroy(item);

            writtenLetters = new List<Card>();
        }
    }

    private Vector3 getTextStartPos()
    {
        return gameObject.transform.position - transform.rotation * new Vector3(normalisedScale() * LetterDistance * (Text.Length / 2.0f), 0, 0);
    }

    public void WriteText()
    {
        if (CardPrefab != null && Text.Length > 0)
        {
            currentPos = getTextStartPos();
            StartCoroutine(WriteLetters());
        }
    }

    IEnumerator WriteLetters()
    {
        for (int i = 0; i < Text.Length; i++)
        {
            char theChar = Text[i];
            writeLetter(theChar);

            yield return new WaitForSeconds(LetterTimer);
        }
        yield return null;
    }

    private void writeLetter(char letter)
    {
        if (letter != ' ')
        {
            Card l = Instantiate(CardPrefab);
            l.transform.rotation = transform.rotation;
            l.transform.position = currentPos;
            l.CardText = letter.ToString();
            l.transform.SetParent(gameObject.transform);
            l.transform.localScale = transform.localScale;
            writtenLetters.Add(l);
        }

        currentPos += transform.rotation * new Vector3(normalisedScale() * LetterDistance, 0, 0);
    }
}
