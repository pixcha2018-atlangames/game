using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    public bool nextScene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire2") || Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("Start");
        }

        if (nextScene)
        {
            SceneManager.LoadScene("Game");
        }
	}
}
