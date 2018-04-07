using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SceneEvent : UnityEvent<Scene>
{
}

public class Scene: MonoBehaviour{

    public Bounds bounds;

    public SceneEvent sceneReady;

    void Start(){
        bounds = new Bounds();
        UpdateBounds();
        sceneReady.Invoke(this);
    }

    public void UpdateBounds(){
        bounds.center = Vector3.zero;
        bounds.size = Vector3.zero;
        UpdateBoundsRec(transform);
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

}