using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenController : MonoBehaviour
{

    public Animation Text;
    public Animation TextBackground;

    public void Awake()
    {
        Text.Stop();
        TextBackground.Stop();
    }

    public void Start()
    {
        StartCoroutine(Appear());
    }

    public void OnDestroy()
    {

    }

    IEnumerator Appear()
    {
        TextBackground.Play();
        yield return new WaitForSeconds(1);
        Text.Play();
        yield return null;
    }
}
