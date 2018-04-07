using UnityEngine;

public class Scene: MonoBehaviour{

    public Bounds bounds;

    void Start(){
        bounds = new Bounds();
        UpdateBounds();
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