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
    public Animator animUnlit;
    public Animator animShadow;
    //public GameObject foot;
    //public GameObject empreintes;
    public float jumpPuissance;
    public float timerJump;
    public float coefTimerJump = 2f;



    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        controller = GetComponent<CharacterController>();
        animUnlit = spriteUnlit.transform.GetComponent<Animator>();
        animShadow = spriteShadow.transform.GetComponent<Animator>();
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
        else if (h > 0f)
        {
            spriteUnlit.flipX = false;
            spriteShadow.flipX = false;
        }

        if (h == 0f && v == 0f)
        {
            animUnlit.SetBool("Walking", false);
            animShadow.SetBool("Walking", false);
        }
        else
        {
            animUnlit.SetBool("Walking", true);
            animShadow.SetBool("Walking", true);
            //var footprint = Instantiate(foot, empreintes.transform);
            //footprint.transform.position = transform.position;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (animUnlit.GetBool("Grelotte") == false)
            {
                animUnlit.SetBool("Grelotte", true);
                animShadow.SetBool("Grelotte", true);
            }
            else
            {
                animUnlit.SetBool("Grelotte", false);
                animShadow.SetBool("Grelotte", false);
            }
        }


        //transform.Translate(new Vector3(h, 0f, v) * vitesse * Time.deltaTime);
        
        //JUMP
        if (Input.GetButtonDown("Jump" + playerID) && timerJump == 0f)
        {
            timerJump = 1f ;
        }

        if (timerJump > 0f)
        {
            if (timerJump - Time.deltaTime * coefTimerJump > 0f)
            {
                timerJump -= Time.deltaTime * coefTimerJump;
            }
            else timerJump = 0f;
        }


        //DEPLACEMENT
        var moveDirection = Vector3.zero;

        if (timerJump != 0f)
        {
            animUnlit.SetBool("Jumping", true);
            animShadow.SetBool("Jumping", true);
            moveDirection.Set(h * (1+timerJump), jumpPuissance * timerJump, v * (1 + timerJump));
        }
        else
        {
            animUnlit.SetBool("Jumping", false);
            animShadow.SetBool("Jumping", false);
            moveDirection.Set(h, 0f, v);
        }

        //moveDirection.Set(h, jumpPuissance * timerJump, v);

        //ADD GRAVITY
        moveDirection.y += gravity;

        //MOVE
        controller.Move(moveDirection * Time.deltaTime);

    }
}
