
using UnityEngine;
    

public class EnvManager {

    private GameObject[] assets;

    public EnvManager(GameObject[] assets){
        this.assets = assets;
    }

	// Update is called once per frame
	void Update () {
		
	}

    public GameObject CreateAsset(string name,Vector3 position){
         GameObject asset = Object.Instantiate(assets[0], position, Quaternion.identity);

         return asset;
    }
}
