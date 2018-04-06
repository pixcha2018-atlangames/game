using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;     

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TextAsset data = Resources.Load<TextAsset>(Path.Combine("Data","story"));
		Debug.Log(data);
        Story loadedData = JsonUtility.FromJson<Story>(data.text);
		Debug.Log(loadedData);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
