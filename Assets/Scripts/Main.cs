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

		//GameObject tree = envManager.CreateAsset("Bush",new Vector3(0,0,0));

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
		}
    }
}
