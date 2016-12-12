using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenController : MonoBehaviour
{

    public Animation Text;
    public Animation TextBackground;
    public Animation MenuText;
    public Animation MenuBackground;

    public void Awake()
    {
        Text.Stop();
        TextBackground.Stop();
        MenuText.Stop();
        MenuBackground.Stop();
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

        MenuBackground.Play();

        yield return new WaitForSeconds(1.5f);

        Text.Play();
        MenuText.Play();
        
        yield return null;
    }
}
