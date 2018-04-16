using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumModifier : MonoBehaviour {

    public float horizObl;
    public float vertObl;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

            Matrix4x4 mat = Camera.main.projectionMatrix;
            mat[0, 2] = horizObl;
            mat[1, 2] = vertObl;
            Camera.main.projectionMatrix = mat;
    }
}
