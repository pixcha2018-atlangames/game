using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float vitesse = 10f;
    public int playerID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var h = Input.GetAxis("Horizontal" + playerID);
        var v = Input.GetAxis("Vertical" + playerID);

        transform.Translate(new Vector3(h, 0f, v) * vitesse * Time.deltaTime);


    }
}
