
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
    

public class EnvManager {

    private Dictionary<string,Scene> assets;


    private Dictionary<string,EnvAssetConf> assetConfs;

    public Move[] players;

    private float lastSpawnTime;

    private float nextSpawnDelay;

    public EnvConf conf;

    public EnvManager(GameObject[] assets, Move[] players){
        this.assets = new Dictionary<string,Scene>();
        this.players = players;
        this.assetConfs = new Dictionary<string,EnvAssetConf>();
        foreach(GameObject a in assets){
            Scene scene = a.GetComponent<Scene>();
            if(scene){
                this.assets.Add(a.name,scene);
            }else{
                throw new Exception("Add Scene component to '"+a.name+"' GameObject");
            }
            
        }
        
    }

     public void Load(string name){

        TextAsset data = Resources.Load<TextAsset>(Path.Combine("Data",name));

        this.conf = JsonUtility.FromJson<EnvConf>(data.text);
        foreach(EnvAssetConf c in conf.assets){
            this.assetConfs.Add(c.id,c);
        }

        nextSpawnDelay = UnityEngine.Random.Range(this.conf.spawnMinDelay,this.conf.spawnMaxDelay);
        lastSpawnTime = Time.time;

    }

	// Update is called once per frame
	public void Update () {

        float currentTime = Time.time;
        if(currentTime-lastSpawnTime > nextSpawnDelay){
            Move[] rndPlayers = Utils.shuffle(players);
            foreach(Move player in rndPlayers){
                if(player.isMoving){
                    spawnNear(player);
                    nextSpawnDelay = UnityEngine.Random.Range(this.conf.spawnMinDelay,this.conf.spawnMaxDelay);
                    lastSpawnTime = currentTime;
                    break;
                }
            }
        }
		
	}

    private void spawnNear(Move player){
        Debug.Log("Spawn near "+player.name);
    }

    public Scene CreateScene(string name,Vector3 position){

        EnvAssetConf assetConf = this.assetConfs[name];

        string goName = assetConf.inherit == null?assetConf.id:assetConf.inherit;
        
        GameObject asset = UnityEngine.Object.Instantiate(this.assets[goName].gameObject, new Vector3(
            position.x,
            UnityEngine.Random.Range(assetConf.minY,assetConf.maxY),
            position.z
        ), Quaternion.identity);
        asset.name = assetConf.id+"["+(++assetConf.index)+"]";
        return asset.GetComponent<Scene>();
    }
}
