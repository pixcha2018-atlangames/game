using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    //PRIVATE
    private GameControl gameControl;

    [Space]
    [Header("Gamepad")]
    public int playerID;

    [Space]
    [Header("Animation")]
    public SpriteRenderer spriteUnlit;
    public Animator animUnlit;
    public Animator anim;

    [Space]
    [Header("Player State")]
    public bool isWalking;
    public bool isGrelotte;
    public bool isRechauffe;
    public bool isLightened;
    public bool isHiding;
    public bool isHappy;
    public bool isMoving;
    public bool isFreeze;
    public bool isFloored;
    public bool isSliding;

    [Space]
    [Header("Movement")]
    public CharacterController controller;
    public float vitesse = 10f;
    public float vitesseJump = 2f;
    public float gravity = 9.81f;
    public Vector3 direction;
    public Queue<float> directionAngleHistory;
    public int directionAngleHistoryLimit = 10;
    public float smoothDirectionAngle;
    [Header("Empreintes")]
    public GameObject empreinteParent;
    public GameObject empreinte;
    public Sprite[] empreintesPNG;
    public float empreinteFreq;
    public float empreinteTimer;
    public float empreinteAngle;
    [Header("Trou")]
    public Sprite[] splashPNG;
    public GameObject splashParticles;
    [Header("Jump")]
    public float jumpPuissance;
    public float timerJump;
    public float coefTimerJump = 2f;
    [Header("Slide")]
    public float slideFriction = 0.3f;
    public float vitesseSlide;
    public float vitesseSlideMax;
    public Vector3 hitNormal;
    public float angleSlide;

    [Space]
    [Header("Raytracing Environement")]
    public Transform colSolAvant;
    public float distToFloor;




    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        directionAngleHistory = new Queue<float>();
        animUnlit = spriteUnlit.transform.GetComponent<Animator>();
        anim = GetComponent<Animator>();
        gameControl = transform.Find("/GameControl").GetComponent<GameControl>();
        empreinteTimer = empreinteFreq;

        if (gameControl.playTest == false)
        {
            if (transform.tag == "Loup")
            {
                animUnlit.SetTrigger("Awake");
            }

            if (transform.tag == "Faon")
            {
                isFreeze = true;
                animUnlit.Play("Gele", 0);
                animUnlit.SetBool("Gele", true);
            }
        }
    }

    // Update is called once per frame
    void Update () {

        if (!isFreeze)
        {
            animUnlit.SetBool("Gele", false);
        }

        if (isGrelotte)
        {
            animUnlit.SetBool("Grelotte", true);
        }
        else if (isHappy)
        {
            animUnlit.SetTrigger("Happy");
        }
        else if (Input.GetButton("Hide" + playerID))
        {
            isHiding = true;

            if (transform.tag == "Loup")
            {
                if (isLightened)
                {
                    animUnlit.SetBool("Hiding", true);
                    animUnlit.Play("HidingNotOk", 0);
                }
                else
                {
                    animUnlit.SetBool("Hiding", true);
                    animUnlit.Play("HidingOk", 0);
                }
            }
            else if (transform.tag == "Faon")
            {
                if (!isLightened)
                {
                    animUnlit.SetBool("Hiding", true);
                    animUnlit.Play("HidingNotOk", 0);
                }
                else
                {
                    animUnlit.SetBool("Hiding", true);
                    animUnlit.Play("HidingOk", 0);
                }
            }

        }
        else if (!isFreeze)
        {
            animUnlit.SetBool("Grelotte", false);
            animUnlit.SetBool("Hiding", false);

            var h = Input.GetAxis("Horizontal" + playerID);
            var v = Input.GetAxis("Vertical" + playerID);

            if (h < 0f)
            {
                spriteUnlit.flipX = true;
            }
            else if (h > 0f)
            {
                spriteUnlit.flipX = false;
            }

            if (h == 0f && v == 0f)
            {
                isWalking = false;
                animUnlit.SetBool("Walking", false);
            }
            else
            {
                isWalking = true;
                animUnlit.SetBool("Walking", true);

                if (timerJump == 0f)
                {
                    if (empreinteTimer <= 0f)
                    {
                        var n = Random.Range(0, empreintesPNG.Length);
                        var newEmpreinte = Instantiate(empreinte, empreinteParent.transform);
                        newEmpreinte.GetComponent<SpriteRenderer>().sprite = empreintesPNG[n];
                        newEmpreinte.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        newEmpreinte.transform.localEulerAngles = new Vector3(-90, 0f, smoothDirectionAngle-90f);

                        empreinteTimer = empreinteFreq;
                    }
                    else empreinteTimer -= Time.deltaTime;
                }
            }

            //JUMP
            if (Input.GetButtonDown("Jump" + playerID) && timerJump == 0f && !isSliding)
            {
                timerJump = 1f;
                anim.SetTrigger("Jump");
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
                moveDirection.Set(h * vitesseJump, jumpPuissance * timerJump, v * vitesseJump);

                if (timerJump < 0.8f && isFloored)
                {
                    animUnlit.SetBool("Jumping", false);
                    moveDirection.Set(h, 0f, v);
                    timerJump = 0f;
                    anim.SetTrigger("Floored");

                    var n = Random.Range(0, splashPNG.Length);
                    var newEmpreinte = Instantiate(empreinte, empreinteParent.transform);
                    newEmpreinte.GetComponent<SpriteRenderer>().sprite = splashPNG[n];
                    newEmpreinte.transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y*10)/10+0.05f, transform.position.z);
                    newEmpreinte.transform.localEulerAngles = new Vector3(-90, 0f, 0f);

                    var newSplash = Instantiate(splashParticles, empreinteParent.transform);
                    newSplash.transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y * 10) / 10 + 0.05f, transform.position.z);



                }
            }
            else
            {
                animUnlit.SetBool("Jumping", false);
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

            Floored();

            // Remove player if he is on a slope
            angleSlide = Vector3.Angle(Vector3.up, hitNormal);
            if (isFloored && angleSlide >= controller.slopeLimit/2)
            {
                isSliding = true;
                animUnlit.SetBool("Slide", true);
                var dir = new Vector3(0f, 0f, 0f);
                dir.x += (1f - hitNormal.y) * hitNormal.x * Mathf.Clamp(vitesseSlide + (angleSlide / 10), 0, vitesseSlideMax);
                dir.z += (1f - hitNormal.y) * hitNormal.z * Mathf.Clamp(vitesseSlide + (angleSlide / 10), 0, vitesseSlideMax);
                controller.Move(dir * Time.deltaTime);

                //if (angleSlide > 0f) // <-- A CHANGER
                //{
                //    spriteUnlit.flipX = true;
                //}
                //else
                //{
                //    spriteUnlit.flipX = false;
                //}
            }
            else
            {
                isSliding = false;
                animUnlit.SetBool("Slide", false);
            }
        }
    }

    public void Floored()
    {
        RaycastHit raycastResult;
        Vector3 raycastDir = colSolAvant.position - transform.position;
        if (Physics.Raycast(transform.position, raycastDir, out raycastResult, distToFloor))
        {
            isFloored = true;
        }
        else
        {
            isFloored = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    public Ray2D GetDirectionRay2D()
    { 
        Quaternion rot = Quaternion.AngleAxis(smoothDirectionAngle, Vector3.up);
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
