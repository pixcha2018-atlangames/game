using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;   

public class StoryManager {

    private Dictionary<string,StoryNode> nodes;

    private Story story;

    private GameObject[] envAssets;

    public StoryManager(){
        nodes = new Dictionary<string,StoryNode>();
    }

    public void Load(string name){

        TextAsset data = Resources.Load<TextAsset>(Path.Combine("Data",name));
        story = JsonUtility.FromJson<Story>(data.text);
        nodes.Clear();

        foreach(StoryNode node in story.nodes){
            nodes.Add(node.id,node);
        }

    }

    public StoryNode GetFirstNode(){
        return GetNode(story.startNode);
    }

    public StoryNode GetNode(string id){
        return nodes[id];
    }

}