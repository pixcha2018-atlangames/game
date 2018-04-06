using UnityEngine;

[System.Serializable]
public class StoryNode
{

    public string id;
    public string[] assets;
    public string[] nodes;

    public override string ToString()
    {
        return base.ToString() + "[" + id + "]";
    }

}