using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mink : MonoBehaviour {


	private Bounds boxA;
	private Bounds boxB;

	private Vector2 penetrationVector;

	// Use this for initialization
	void Start () {
		boxA = new Bounds(
			new Vector3(0,0,0),
			new Vector3(1f,1f,0f)
		);
		boxB = new Bounds(
			new Vector3(0,0,0),
			new Vector3(5f,5f,0f)
		);
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		boxA.center = ray.origin;

		Bounds2D box2dA = Bounds2D.boundsXYTo2D(boxA);
		Bounds2D box2dB = Bounds2D.boundsXYTo2D(boxB);

		// get the minkowski difference between the two
		Bounds2D md = box2dB.minkowskiDifference(box2dA);
		penetrationVector = new Vector2();
		if (md.min.x <= 0 &&
			md.max.x >= 0 &&
			md.min.y <= 0 &&
			md.max.y >= 0)
		{
			// find the penetration depth
			penetrationVector = md.closestPointOnBoundsToPoint(Vector2.zero);
			box2dA.center += penetrationVector;
		}

		boxA.center = new Vector2(
			box2dA.center.x,
			box2dA.center.y
		);
	}


	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(boxB.center,boxB.size);

		Gizmos.color = Color.blue;
		Gizmos.DrawCube(boxA.center,boxA.size);

		Gizmos.color = Color.blue;
		Debug.DrawLine(new Vector3(0,0,-1), new Vector3(penetrationVector.x, penetrationVector.y,-1));

	}
}
