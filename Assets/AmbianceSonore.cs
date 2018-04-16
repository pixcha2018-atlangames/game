using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceSonore : MonoBehaviour {

    public AudioClip[] sounds;
    public bool loop;
    public float delayMin;
    public float delayMax;
    public float delayTimer;
    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (loop)
        {
            delayTimer -= Time.deltaTime;

            if (!audioSource.isPlaying && delayTimer <= 0f)
            {
                var n = Random.Range(0, sounds.Length);
                audioSource.clip = sounds[n];
                audioSource.enabled = true;
                audioSource.Play();
                delayTimer = Random.Range(delayMin,delayMax);
            }
        }
    }
}
