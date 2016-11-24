using UnityEngine;
using System.Collections;

[System.Serializable]
public struct ScreenItems
{
    public string Name;
}

[RequireComponent(typeof(Animator))]
public class ScreenSwitcher : MonoBehaviour
{

    public ScreenItems[] Screens;
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
            throw new System.Exception("No Animator found !");
    }

    public void SwitchTo(string name)
    {
        for (int i = 0; i < Screens.Length; i++)
        {
            if (string.Equals(name, Screens[i].Name))
                animator.SetInteger("ScreenId", i);
        }
    }
}
