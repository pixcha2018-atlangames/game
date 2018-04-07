
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;


public class EnvManager
{

    private Dictionary<string, Scene> assets;


    private Dictionary<string, EnvAssetConf> assetConfs;

    public Move[] players;

    private float lastSpawnTime;

    private float nextSpawnDelay;

    public EnvConf conf;

    private List<Scene> scenes;

    private Move selectedPlayer;

    private bool lockSpawning = false;

    private Scene currentScene;

    private int sceneIndex = 0;

    private GameObject[] orderScenes;

    private Queue<Scene> delayedScenes;

    private float viewTimeout = 10f;

    public EnvManager(GameObject[] assets, Move[] players)
    {
        this.assets = new Dictionary<string, Scene>();
        this.players = players;
        this.delayedScenes = new Queue<Scene>();
        this.scenes = new List<Scene>();
        this.assetConfs = new Dictionary<string, EnvAssetConf>();
        this.orderScenes = assets;
        /*foreach(GameObject a in assets){
            Scene scene = a.GetComponent<Scene>();
            if(scene){
                this.assets.Add(a.name,scene);
            }else{
                throw new Exception("Add Scene component to '"+a.name+"' GameObject");
            }
            
        }*/

    }

    public void Load(string name)
    {

        TextAsset data = Resources.Load<TextAsset>(Path.Combine("Data", name));

        this.conf = JsonUtility.FromJson<EnvConf>(data.text);
        foreach (EnvAssetConf c in conf.assets)
        {
            this.assetConfs.Add(c.id, c);
        }

        nextSpawnDelay = UnityEngine.Random.Range(this.conf.spawnMinDelay, this.conf.spawnMaxDelay);
        lastSpawnTime = Time.time;

    }

    // Update is called once per frame
    public void Update(Bounds cameraBounds)
    {

        float currentTime = Time.time;
        bool allScenesVisited = true;

        currentScene = null;

        foreach (Scene scene in this.scenes)
        {
            if (scene.state == "ready")
            {
                if (!scene.isView)
                {
                    scene.isView = cameraBounds.Intersects(scene.visible2Bounds);
                    if (!scene.isView && currentTime - scene.viewLastTime > viewTimeout)
                    {
                        Debug.Log(scene.name + " timeout : reset");
                        scene.state = "initialized";
                        scene.gameObject.SetActive(false);
                        this.delayedScenes.Enqueue(scene);
                        lockSpawning = false;
                    }

                }
                else
                {
                    scene.CheckVisit(cameraBounds);
                    bool overlaps = cameraBounds.Intersects(scene.bounds);
                    if (overlaps) currentScene = scene;
                }

            }
        }

        foreach (Scene scene in this.scenes)
        {
            //Debug.Log(scene.state);
            //Debug.Log(selectedPlayer);


            allScenesVisited = allScenesVisited && scene.isVisited;
            //Debug.Log("scene allScenesVisited"+allScenesVisited);
            //Debug.Log("scene.isVisited"+scene.isVisited);
            bool onBorder = currentScene == null ? true : !cameraBounds.Intersects(currentScene.visibleBounds);
            if (scene.state == "initialized" && selectedPlayer != null && onBorder)
            {


                Ray2D ray = selectedPlayer.GetDirectionRay2D();

                Bounds2D cameraBounds2D = Bounds2D.boundsXZTo2D(cameraBounds);

                Vector2 p;

                Vector3 penetrationVector = new Vector3();

                if (cameraBounds2D.intersectionWithRay2D(ray, out p))
                {
                    Vector3 diff = scene.bounds.center - scene.transform.position;
                    Vector3 initialPos = new Vector3(p.x, 0, p.y);
                    scene.bounds.center = initialPos;

                    Bounds currentBounds = new Bounds(
                        cameraBounds.center,
                        cameraBounds.size
                    );
                    if (currentScene != null)
                    {
                        currentBounds.Encapsulate(currentScene.bounds);
                    }

                    Bounds2D currentBounds2D = Bounds2D.boundsXZTo2D(currentBounds);

                    Bounds2D md = Bounds2D.minkowskiDifference(currentBounds2D, Bounds2D.boundsXZTo2D(scene.bounds));
                    penetrationVector = md.closestPointOnBoundsToPoint(Vector2.zero);

                    scene.bounds.center += new Vector3(penetrationVector.x, 0, penetrationVector.y);
                    scene.visibleBounds.center = scene.bounds.center;
                    scene.visible2Bounds.center = scene.bounds.center;
                    scene.transform.position = scene.bounds.center - diff;

                    bool isOk = true;

                    foreach (Scene aScene in scenes)
                    {
                        if (aScene != scene && aScene.bounds.Intersects(scene.bounds))
                        {
                            isOk = false;
                            break;
                        }
                    }

                    if (isOk)
                    {
                        scene.state = "ready";
                        scene.viewLastTime = Time.time;
                        selectedPlayer = null;
                        scene.gameObject.SetActive(true);
                    }
                }
            }

        }

        if (allScenesVisited && lockSpawning)
        {
            Debug.Log("allScenesVisited " + allScenesVisited);
            Debug.Log("lockSpawning " + lockSpawning);
            nextSpawnDelay = UnityEngine.Random.Range(this.conf.spawnMinDelay, this.conf.spawnMaxDelay);
            lastSpawnTime = currentTime;
            lockSpawning = false;
        }

        if (!lockSpawning && currentTime - lastSpawnTime > nextSpawnDelay)
        {

            Move[] rndPlayers = Utils.shuffle(players);
            foreach (Move player in rndPlayers)
            {
                //Debug.Log(player.name+" is moving"+player.isMoving);
                if (player.isMoving)
                {
                    Debug.Log("create scene ");
                    selectedPlayer = player;
                    //string name = orderScenes[sceneIndex].name;

                    if (delayedScenes.Count == 0)
                    {
                        if (sceneIndex < orderScenes.Length)
                        {
                            this.scenes.Add(CreateScene(orderScenes[sceneIndex], new Vector3(0, 0, 0)));
                            sceneIndex++;
                        }
                    }
                    else
                    {
                        delayedScenes.Dequeue();
                    }

                    lockSpawning = true;
                    break;
                }
            }

        }

    }
    /* 
        private void spawnSceneNear(string name,Move player, Bounds cameraBounds){
            Debug.Log("Spawn near "+player.name);
            Vector3 penetrationVector = new Vector3();

            CreateScene(name,new Vector3(0,0,0)).sceneReady.AddListener((Scene scene) => {
                Ray2D ray = player.GetDirectionRay2D();
                Bounds2D cameraBounds2D = Bounds2D.boundsXZTo2D(cameraBounds);

                Vector2 p;

                if(cameraBounds2D.intersectionWithRay2D(ray, out p)){
                    Vector3 diff = scene.bounds.center - scene.transform.position;
                    Vector3 initialPos = new Vector3(p.x,0,p.y);
                    scene.bounds.center = initialPos;
                    Bounds2D md = Bounds2D.minkowskiDifference(cameraBounds2D,Bounds2D.boundsXZTo2D(scene.bounds));
                    penetrationVector = md.closestPointOnBoundsToPoint(Vector2.zero);
                    scene.bounds.center += new Vector3(penetrationVector.x,0,penetrationVector.y);
                    scene.transform.position = scene.bounds.center - diff;
                }
            });
        }
    */

    public Scene CreateScene(string name, Vector3 position)
    {

        EnvAssetConf assetConf = this.assetConfs[name];

        string goName = assetConf.inherit == null ? assetConf.id : assetConf.inherit;

        GameObject asset = UnityEngine.Object.Instantiate(this.assets[goName].gameObject, new Vector3(
            position.x,
            UnityEngine.Random.Range(assetConf.minY, assetConf.maxY),
            position.z
        ), Quaternion.identity);
        asset.name = assetConf.id + "[" + (++assetConf.index) + "]";
        return asset.GetComponent<Scene>();
    }

    public Scene CreateScene(GameObject obj, Vector3 position)
    {

        GameObject asset = UnityEngine.Object.Instantiate(obj, new Vector3(
            position.x,
            0,//UnityEngine.Random.Range(assetConf.minY,assetConf.maxY),
            position.z
        ), Quaternion.identity);
        return asset.GetComponent<Scene>();
    }
}
