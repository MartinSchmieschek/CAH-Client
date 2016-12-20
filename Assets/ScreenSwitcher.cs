using UnityEngine;
using System.Collections;

[System.Serializable]
public struct ScreenItems
{
    public string Name;
    public Camera GameObject;
}

[RequireComponent(typeof(Camera))]
public class ScreenSwitcher : MonoBehaviour
{

    public ScreenItems[] Screens;

    public void SwitchTo(string name)
    {
        for (int i = 0; i < Screens.Length; i++)
        {
            if (string.Equals(name, Screens[i].Name))
            {
                if (Screens[i].GameObject != null)
                {
                    transform.position = Screens[i].GameObject.transform.position;
                    transform.rotation = Screens[i].GameObject.transform.rotation;
                }
            }
                
        }
    }
}
