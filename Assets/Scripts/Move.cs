using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour { 

    public float vitesse = 10f;
    public int playerID;
    public CharacterController controller;
    public float gravity = 9.81f;
    public SpriteRenderer spriteUnlit;
    public SpriteRenderer spriteShadow;
    public float cold;


	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        var h = Input.GetAxis("Horizontal" + playerID);
        var v = Input.GetAxis("Vertical" + playerID);

        if (h < 0f)
        {
            spriteUnlit.flipX = true;
            spriteShadow.flipX = true;

        }
        if (h > 0f)
        {
            spriteUnlit.flipX = false;
            spriteShadow.flipX = false;
        }


        //transform.Translate(new Vector3(h, 0f, v) * vitesse * Time.deltaTime);

        //DEPLACEMENT
        var moveDirection = Vector3.zero;
        moveDirection.Set(h, 0f, v);
        moveDirection *= vitesse;

        //ADD GRAVITY
        moveDirection.y += gravity;

        //MOVE
        controller.Move(moveDirection * Time.deltaTime);


    }
}
