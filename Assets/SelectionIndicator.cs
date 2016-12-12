using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class SelectionIndicator : MonoBehaviour {

    private MeshRenderer mr;
    private bool activated = false;

	// Use this for initialization
	void Awake () {


        mr = transform.GetComponent<MeshRenderer>();
		
	}

    public void Activate()
    {
        if (mr == null)
            updateMeshRenderer();

        if (!activated)
        {
            activated = true;
            mr.enabled = true;
        }
    }

    public void Deactivate()
    {
        if (mr == null)
            updateMeshRenderer();


        if (activated)
        {
            activated = false;
            mr.enabled = false;
        }
    }

    private void updateMeshRenderer ()
    {
        mr = transform.GetComponent<MeshRenderer>();
    }




}
