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

	private Vector3[] cameraBounds;

	// Use this for initialization
	void Start () {
		cameraBounds = new Vector3[4];

		storyManager = new StoryManager();
		storyManager.Load(storyName);

		envManager = new EnvManager(envAssets);

		Debug.Log(storyManager.GetFirstNode());

		UpdateCameraBounds();
		Debug.Log("---------");

		for(int i = 0, c=cameraBounds.Length;i<c;i++){
			Vector3 p = cameraBounds[i];
			
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.name = "Sphere "+i;
			sphere.transform.position = new Vector3(p.x,p.y,p.z);
			Debug.Log(i+" ----");
			Debug.Log(p);
			Debug.Log(sphere.transform.position);
		}
		Debug.Log("---------");


	}

	void UpdateCameraBounds(){
		MeshCollider collider = plane.GetComponent<MeshCollider>();
		int i = 0;
		for(int x = 0; x < 2;x++){
			for(int y = 0; y < 2;y++){
				
				Ray ray = camera.ViewportPointToRay(new Vector3(x, y, 0));
				RaycastHit hit;
				collider.Raycast(ray,out hit,100f);
				cameraBounds[i] =  hit.point;
				i++;
			}
		}
	}


	
	// Update is called once per frame
	void Update () {
		//UpdateCameraBounds();

	}

	void OnDrawGizmosSelected()
    {
		foreach(Vector3 p in cameraBounds){
			Gizmos.color = Color.yellow;
        	Gizmos.DrawSphere(p, 0.1F);
		}
    }
}
