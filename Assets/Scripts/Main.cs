using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;     

public class Main : MonoBehaviour {

	private StoryManager storyManager;

	private EnvManager envManager;
	
	private string storyName = "story";

	public GameObject[] envAssets;
	public PlayerControl[] players;
	public Camera currentCamera;
	public GameObject plane;

	private Vector3[] cameraLimits;

	private Bounds cameraBounds;

	private Scene decor;
	private Bounds2D md;

	private Vector3 penetrationVector;

	// Use this for initialization
	void Start () {
		
		cameraLimits = new Vector3[4];
		cameraBounds = new Bounds();

		storyManager = new StoryManager();
		storyManager.Load(storyName);

		envManager = new EnvManager(envAssets,players);
		envManager.Load("env");

		Debug.Log(storyManager.GetFirstNode());

		UpdateCameraBounds();

		/*decor = envManager.CreateScene("Decor01",new Vector3(0,0,0));

		penetrationVector = new Vector3();

		decor.sceneReady.AddListener((Scene scene) => {

			Vector3 diff = scene.bounds.center - scene.transform.position;
			scene.bounds.center = players[0].transform.position;
			md = Bounds2D.minkowskiDifference(Bounds2D.boundsXZTo2D(cameraBounds),Bounds2D.boundsXZTo2D(decor.bounds));
			penetrationVector = md.closestPointOnBoundsToPoint(Vector2.zero);
			scene.bounds.center += new Vector3(penetrationVector.x,0,penetrationVector.y);
			scene.transform.position = scene.bounds.center - diff;

		});*/

	}

	void UpdateCameraBounds(){
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
		s.y = 0.1f;
		cameraBounds.size = s;

	}

	// Update is called once per frame
	void Update () {
		UpdateCameraBounds();
		envManager.Update(cameraBounds);
	}

	void DrawSegment(Segment2D seg){
		Gizmos.DrawSphere(new Vector3(seg.a.x,0,seg.a.y),0.5f);
		Gizmos.DrawSphere(new Vector3(seg.b.x,0,seg.b.y),0.5f);
	}

	void OnDrawGizmosSelected()
    {
		if(cameraLimits!=null){
			
		

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(cameraBounds.center,cameraBounds.size);

			Gizmos.color = Color.blue;

			Ray2D ray = players[0].GetDirectionRay2D();

			Bounds2D cameraBounds2D = Bounds2D.boundsXZTo2D(cameraBounds);

			Vector2 p;

			if(Bounds2D.boundsXZTo2D(cameraBounds).intersectionWithRay2D(ray, out p)){
				Gizmos.color = Color.black;
				Gizmos.DrawSphere(new Vector3(p.x,0,p.y),1f);
			}

		}
    }
}
