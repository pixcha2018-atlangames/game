using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCamera : MonoBehaviour {

    public GameObject loupGO;
    public GameObject faonGO;
    public float heigthCam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var newPositionCam = faonGO.transform.position + (loupGO.transform.position - faonGO.transform.position)/2;
        transform.position = newPositionCam;
        
        
	}
}
