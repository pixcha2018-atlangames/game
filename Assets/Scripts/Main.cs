using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;     

public class Main : MonoBehaviour {

	private StoryManager storyManager;

	private EnvManager envManager;
	
	private string storyName = "story";

	public GameObject[] envAssets;
	public Move[] players;
	public Camera currentCamera;
	public GameObject plane;

	private Vector3[] cameraLimits;

	private Bounds cameraBounds;

	private Scene decor;
	private Bounds md;

	// Use this for initialization
	void Start () {
		
		cameraLimits = new Vector3[4];
		cameraBounds = new Bounds();

		storyManager = new StoryManager();
		storyManager.Load(storyName);

		envManager = new EnvManager(envAssets,players);
		envManager.Load("env");

		Debug.Log(storyManager.GetFirstNode());

		UpdatecameraBounds();

		decor = envManager.CreateScene("Decor01",new Vector3(0,0,0));
		decor.UpdateBounds();

		md = Utils.minkowskiDifference(cameraBounds,decor.bounds);
		Vector3 penetrationVector = new Vector3();

			
		// find the penetration depth
		penetrationVector = md.ClosestPoint(Vector3.zero);
			
		decor.bounds.center += penetrationVector;


	}

	void UpdatecameraBounds(){
		MeshCollider collider = plane.GetComponent<MeshCollider>();
		int i = 0;
		for(int x = 0; x < 2;x++){
			for(int y = 0; y < 2;y++){
				
				Ray ray = currentCamera.ViewportPointToRay(new Vector3(x, y, 0));
				RaycastHit hit;
				collider.Raycast(ray,out hit,100f);
				cameraLimits[i] =  hit.point;
				i++;
			}
		}

		cameraBounds = Utils.CreateBounds(cameraLimits);
		Vector3 s = cameraBounds.size;
		s.y = 10f;
		cameraBounds.size = s;

	}

	// Update is called once per frame
	void Update () {
		UpdatecameraBounds();
		envManager.Update();
	}

	void OnDrawGizmosSelected()
    {
		if(cameraLimits!=null){
			for(int i = 0, c=cameraLimits.Length;i<c;i++){
				Vector3 p = cameraLimits[i];
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(p, 1F);
			}

			Gizmos.DrawCube(cameraBounds.center,cameraBounds.size);
			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(decor.bounds.center,decor.bounds.size);
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(md.center,md.size);
		}
    }
}
