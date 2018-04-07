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
    public bool isWalking;
    public bool isGrelotte;
    public bool isRechauffe;
    public bool isLightened;
    public bool isHiding;
    public bool isHappy;
    public Vector3 direction;
    public Queue<float> directionAngleHistory = new Queue<float>();
    public int directionAngleHistoryLimit = 10;
    public float smoothDirectionAngle;
    public bool isMoving;
    public bool isFreeze;



    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        directionAngleHistory = new Queue<float>();
        animUnlit = spriteUnlit.transform.GetComponent<Animator>();
        animShadow = spriteShadow.transform.GetComponent<Animator>();

        if (transform.tag == "Loup")
        {
            animUnlit.SetTrigger("Awake");
            animShadow.SetTrigger("Awake");
        }

        if (transform.tag == "Faon")
        {
            isFreeze = true;
            animUnlit.Play("Gele",0);
            animShadow.Play("Gele", 0);
            animUnlit.SetBool("Gele", true);
            animShadow.SetBool("Gele", true);
        }
    }

    // Update is called once per frame
    void Update () {

        if (!isFreeze)
        {
            animUnlit.SetBool("Gele", false);
            animShadow.SetBool("Gele", false);
        }

        if (isGrelotte)
        {
            animUnlit.SetBool("Grelotte", true);
            animShadow.SetBool("Grelotte", true);
        }
        else if (isHappy)
        {
            animUnlit.SetTrigger("Happy");
            animShadow.SetTrigger("Happy");
        }
        else if (Input.GetButton("Hide" + playerID))
        {
            isHiding = true;

            if (transform.tag == "Loup")
            {
                if (isLightened)
                {
                    animUnlit.SetBool("Hiding", true);
                    animShadow.SetBool("Hiding", true);

                    animUnlit.Play("HidingNotOk", 0);
                    animShadow.Play("HidingNotOk", 0);
                }
                else
                {
                    animUnlit.SetBool("Hiding", true);
                    animShadow.SetBool("Hiding", true);

                    animUnlit.Play("HidingOk", 0);
                    animShadow.Play("HidingOk", 0);
                }
            }
            else if (transform.tag == "Faon")
            {
                if (!isLightened)
                {
                    animUnlit.SetBool("Hiding", true);
                    animShadow.SetBool("Hiding", true);

                    animUnlit.Play("HidingNotOk", 0);
                    animShadow.Play("HidingNotOk", 0);
                }
                else
                {
                    animUnlit.SetBool("Hiding", true);
                    animShadow.SetBool("Hiding", true);

                    animUnlit.Play("HidingOk", 0);
                    animShadow.Play("HidingOk", 0);
                }
            }

        }
        else if (!isFreeze)
        {
            animUnlit.SetBool("Grelotte", false);
            animShadow.SetBool("Grelotte", false);

            animUnlit.SetBool("Hiding", false);
            animShadow.SetBool("Hiding", false);

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
                isWalking = false;
                animUnlit.SetBool("Walking", false);
                animShadow.SetBool("Walking", false);
            }
            else
            {
                isWalking = true;
                animUnlit.SetBool("Walking", true);
                animShadow.SetBool("Walking", true);
                //var footprint = Instantiate(foot, empreintes.transform);
                //footprint.transform.position = transform.position;
            }

            //if (Input.GetButtonDown("Fire1"))
            //{
            //    if (animUnlit.GetBool("Grelotte") == false)
            //    {
            //        animUnlit.SetBool("Grelotte", true);
            //        animShadow.SetBool("Grelotte", true);
            //    }
            //    else
            //    {
            //        animUnlit.SetBool("Grelotte", false);
            //        animShadow.SetBool("Grelotte", false);
            //    }
            //}

            //JUMP
            if (Input.GetButtonDown("Jump" + playerID) && timerJump == 0f)
            {
                timerJump = 1f;
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
                moveDirection.Set(h * (1 + timerJump), jumpPuissance * timerJump, v * (1 + timerJump));
            }
            else
            {
                animUnlit.SetBool("Jumping", false);
                animShadow.SetBool("Jumping", false);
                moveDirection.Set(h, 0f, v);
            }

            //ADD GRAVITY
            moveDirection.y += gravity;

            //MOVE
            controller.Move(moveDirection * Time.deltaTime);

            direction = moveDirection;

             if (directionAngleHistory != null && directionAngleHistory.Count > directionAngleHistoryLimit)
            {
                directionAngleHistory.Dequeue();
            }

            isMoving = direction.x != 0 || direction.z != 0;

            if (isMoving)
            {
                float angle = Vector2.SignedAngle(new Vector2(direction.x, direction.z), new Vector2(0, 1));
                directionAngleHistory.Enqueue(angle);
            }

            smoothDirectionAngle = Utils.GetAverage(directionAngleHistory.ToArray());
        }
    }

    public Ray2D GetDirectionRay2D()
    { 
        //Quaternion rot = Quaternion.AngleAxis(smoothDirectionAngle, Vector3.up);
        // that's a local direction vector that points in forward direction but also 45 upwards.
        //Vector3 dir = rot * new Vector3(1,0,1);
        Vector3 dir = new Vector3(direction.x, 0, direction.z);

        /*float radians = smoothDirectionAngle * Mathf.Deg2Rad; 
        Vector3 dir = new Vector3(Mathf.Cos(radians),0, Mathf.Sin(radians));

        Debug.Log("smoothDirectionAngle :"+smoothDirectionAngle);
        Debug.Log("direction :"+Vector2.SignedAngle(new Vector2(direction.x,direction.z),new Vector2(0,1)));
        */

        return new Ray2D(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(dir.x, dir.z)
        );
    }
}
