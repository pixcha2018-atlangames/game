
using UnityEngine;
using System.Collections.Generic;
using System.IO;
    

public class EnvManager {

    private Dictionary<string,GameObject> assets;


    private Dictionary<string,EnvAssetConf> assetConfs;

    public EnvManager(GameObject[] assets){
        this.assets = new Dictionary<string,GameObject>();
        this.assetConfs = new Dictionary<string,EnvAssetConf>();
        foreach(GameObject a in assets){
            this.assets.Add(a.name,a);
        }
    }

     public void Load(string name){

        TextAsset data = Resources.Load<TextAsset>(Path.Combine("Data",name));

        EnvConf conf = JsonUtility.FromJson<EnvConf>(data.text);
        foreach(EnvAssetConf c in conf.assets){
            Debug.Log(c.id+" "+c);
            this.assetConfs.Add(c.id,c);
        }

    }

	// Update is called once per frame
	void Update () {
		
	}

    public GameObject CreateAsset(string name,Vector3 position){

        Debug.Log("CreateAsset "+name);
        Debug.Log(this.assetConfs.ContainsKey(name));

        EnvAssetConf assetConf = this.assetConfs[name];

        string goName = assetConf.inherit == null?assetConf.id:assetConf.inherit;
        
        GameObject asset = Object.Instantiate(this.assets[goName], new Vector3(
            position.x,
            Random.Range(assetConf.minY,assetConf.maxY),
            position.z
        ), Quaternion.identity);
        asset.name = assetConf.id+"["+(++assetConf.index)+"]";
        return asset;
    }
}
