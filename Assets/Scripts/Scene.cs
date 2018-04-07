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
        Debug.Log(bounds);
    }

    void UpdateBoundsRec(Transform node){
        foreach (Transform child in node)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if(renderer != null){
                bounds.Encapsulate(renderer.bounds);
            }
            UpdateBoundsRec(child);
        }
    }

}