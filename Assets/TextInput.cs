using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TextInput : MonoBehaviour {

    public UnityEvent OnReturn;
    public string Text { get; private set; }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (OnReturn != null)
            {
                OnReturn.Invoke();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            //KeyCode d = Input
        }
	
	}
}
