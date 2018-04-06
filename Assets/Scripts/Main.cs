using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;     

public class Main : MonoBehaviour {

	private StoryManager storyManager;

	private EnvManager envManager;
	
	private string storyName = "story";

	public GameObject[] envAssets;
	public GameObject[] players;
	public Camera camera;
	public GameObject plane;

	// Use this for initialization
	void Start () {
		storyManager = new StoryManager();
		storyManager.Load(storyName);

		envManager = new EnvManager(envAssets);

		Debug.Log(storyManager.GetFirstNode());


		test(0,0);
		test(0,1);
		test(1,0);
		test(1,1);

	}

	void test(float x, float y){
		Ray ray = camera.ViewportPointToRay(new Vector3(x, y, Vector3.Distance(camera.transform.position,new Vector3(0,0,0))));
		MeshCollider collider = plane.GetComponent<MeshCollider>();
		RaycastHit hit;
		collider.Raycast(ray,out hit,100f);
		Vector3 p = hit.point;
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = p;

	}
	
	// Update is called once per frame
	void Update () {
		if(camera == null) return;

		
	}

	void OnDrawGizmosSelected()
    {
        /*Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 0.1F);*/
    }
}
