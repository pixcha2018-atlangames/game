using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCamera : MonoBehaviour {

    public GameObject loupGO;
    public GameObject faonGO;
    public float heigthCam;
    public float cold = 10f;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var newPositionCam = faonGO.transform.position + (loupGO.transform.position - faonGO.transform.position)/2;
        //transform.position = newPositionCam;

        var distPersos = Vector3.Distance(loupGO.transform.position, faonGO.transform.position);

        transform.position = new Vector3(newPositionCam.x, Mathf.Clamp(distPersos / 3, 1, 4), newPositionCam.z - Mathf.Clamp(distPersos / 3, 0, 4));

        if (distPersos > 1f)
        {
            cold -= Time.deltaTime * distPersos/5;
        }

        if (cold < 0f)
        {
            Debug.Log("cold !");
        }
    }
}
