using UnityEngine;

[System.Serializable]
public class StoryNode {

	public string id;
    public string asset;
    public string[] nodes;

    public override string ToString(){
        return id +" "+asset;
    }
}