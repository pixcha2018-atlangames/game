using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolOiseau : MonoBehaviour {

    public Vector3 destination;
    public float vitesse;
    public bool isMoving;
    public Animator anim;

    public float distMax = 10f;
    public GameObject loupGO;
    public GameObject faonGO;
    public float distLoup;
    public float distFaon;

    public bool isDegel;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        loupGO = transform.Find("/Loup").gameObject;
        faonGO = transform.Find("/Faon").gameObject;

    }

    // Update is called once per frame
    void Update () {
        if(!isDegel)
        {
            distLoup = Vector3.Distance(transform.position, loupGO.transform.position);
            distFaon = Vector3.Distance(transform.position, faonGO.transform.position);

            if (distFaon < distMax && distLoup < distMax && loupGO.GetComponent<Move>().isRechauffe)
            {
                anim.SetTrigger("Fonte");
                isDegel = true;
            }
        }



        if (isMoving)
        {
            transform.Translate(destination * vitesse * Time.deltaTime);
        }
        else
        {
            var clipInfo = anim.GetCurrentAnimatorClipInfo(0);
            var animName = clipInfo[0].clip.name;

            if (animName == "Vol")
            {
                isMoving = true;
            }
        }
	}



}
