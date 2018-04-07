using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SceneEvent : UnityEvent<Scene>
{
}

public class Scene: MonoBehaviour{

    public Bounds bounds;
    public Bounds visibleBounds;

    public SceneEvent sceneReady;

    public string state = "none";

    public bool isVisited = false;

    void Start(){
        bounds = new Bounds();
        UpdateBounds();
        this.gameObject.SetActive(false);
        state = "initialized";
        sceneReady.Invoke(this);
    }

    public void UpdateBounds(){
        bounds.center = Vector3.zero;
        bounds.size = Vector3.zero;
        UpdateBoundsRec(transform);
        visibleBounds = new Bounds(
            bounds.center,
            bounds.size * 0.7f
        );
        bounds.size *= 1.5f;
    }

    public bool CheckVisit(Bounds cameraBounds){
        if(!this.isVisited){
            this.isVisited = cameraBounds.Intersects(this.visibleBounds);
            if(this.isVisited){
                Debug.Log(this.name+" is visited");
            }
            return this.isVisited;
        }
        return isVisited;
    }

    void UpdateBoundsRec(Transform node){
        Renderer renderer = node.GetComponent<Renderer>();
        if(renderer != null){
            bounds.Encapsulate(renderer.bounds);
        }
        foreach (Transform child in node)
        {
            UpdateBoundsRec(child);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center,bounds.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(visibleBounds.center,visibleBounds.size);
    }

}