using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpriteShadow : MonoBehaviour {

    public SpriteRenderer original;
    public SpriteRenderer shadow;

    // Use this for initialization
    void Start () {
        original = transform.parent.GetComponent<SpriteRenderer>();
        shadow = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        shadow.sprite = original.sprite;
        shadow.flipX = original.flipX;
	}
}
