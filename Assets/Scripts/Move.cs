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

    public Vector3 direction;

    public Queue<float> directionAngleHistory;

    public int directionAngleHistoryLimit = 10;

    public float smoothDirectionAngle;

    public bool isMoving;


	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        directionAngleHistory = new Queue<float>();
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
        direction= Vector3.zero;
        direction.Set(h, 0f, v);
        direction *= vitesse;

        //ADD GRAVITY
        direction.y += gravity;

        //MOVE
        controller.Move(direction * Time.deltaTime);

        if(directionAngleHistory != null && directionAngleHistory.Count > directionAngleHistoryLimit){
            directionAngleHistory.Dequeue();
        }

        isMoving = direction.x != 0 && direction.z != 0;

        if(isMoving){
            float angle = Vector2.SignedAngle(new Vector2(direction.x,direction.z),new Vector2(0,1));
            directionAngleHistory.Enqueue(angle);
        }

        smoothDirectionAngle = Utils.GetAverage(directionAngleHistory.ToArray());
    }

    
}


