using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;     

public class Main : MonoBehaviour {

	private StoryManager storyManager;

	private string storyName = "story";

	// Use this for initialization
	void Start () {
		storyManager = new StoryManager();
		storyManager.Load(storyName);

		Debug.Log(storyManager.GetFirstNode());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
