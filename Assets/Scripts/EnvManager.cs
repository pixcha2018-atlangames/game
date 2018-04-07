
using UnityEngine;
using System.Collections.Generic;
using System.IO;
    

public class EnvManager {

    private Dictionary<string,GameObject> assets;


    private Dictionary<string,EnvAssetConf> assetConfs;

    public Move[] players;

    private float lastSpawnTime;

    private float nextSpawnDelay;

    public EnvConf conf;

    public EnvManager(GameObject[] assets, Move[] players){
        this.assets = new Dictionary<string,GameObject>();
        this.players = players;
        this.assetConfs = new Dictionary<string,EnvAssetConf>();
        foreach(GameObject a in assets){
            this.assets.Add(a.name,a);
        }

        
        
    }

     public void Load(string name){

        TextAsset data = Resources.Load<TextAsset>(Path.Combine("Data",name));

        this.conf = JsonUtility.FromJson<EnvConf>(data.text);
        foreach(EnvAssetConf c in conf.assets){
            this.assetConfs.Add(c.id,c);
        }

        nextSpawnDelay = Random.Range(this.conf.spawnMinDelay,this.conf.spawnMaxDelay);
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
                    nextSpawnDelay = Random.Range(this.conf.spawnMinDelay,this.conf.spawnMaxDelay);
                    lastSpawnTime = currentTime;
                    break;
                }
            }
        }
		
	}

    private void spawnNear(Move player){
        Debug.Log("Spawn near "+player.name);
    }

    public GameObject CreateAsset(string name,Vector3 position){

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
