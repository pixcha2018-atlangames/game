using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCamera : MonoBehaviour {

    public GameObject loupGO;
    public GameObject faonGO;
    public float heigthCam;
    public float distPersos;
    public float distCold = 0.6f;
    public float cold = 10f;

    public bool isRapace;
    public GameObject rapace;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isRapace)
        {
            rapace.GetComponent<RapaceBehavior>();
        }

        var newPositionCam = faonGO.transform.position + (loupGO.transform.position - faonGO.transform.position)/2;
        //transform.position = newPositionCam;

        distPersos = Vector3.Distance(loupGO.transform.position, faonGO.transform.position);

        transform.position = new Vector3(newPositionCam.x, Mathf.Clamp(distPersos / 3, 1, 4), newPositionCam.z - Mathf.Clamp(distPersos / 3, 0, 4));

        if (distPersos > distCold)
        {
            cold -= Time.deltaTime * distPersos/5;
        }
        else
        {
            if (loupGO.GetComponent<Move>().isWalking == false && faonGO.GetComponent<Move>().isWalking == false)
            {
                loupGO.GetComponent<Move>().animUnlit.SetBool("Rechauffe", true);
                loupGO.GetComponent<Move>().animShadow.SetBool("Rechauffe", true);
                faonGO.GetComponent<Move>().animUnlit.SetBool("Rechauffe", true);
                faonGO.GetComponent<Move>().animShadow.SetBool("Rechauffe", true);

                loupGO.GetComponent<Move>().isRechauffe = true;
                faonGO.GetComponent<Move>().isRechauffe = true;

                cold = 10f;

            }
            else
            {
                loupGO.GetComponent<Move>().animUnlit.SetBool("Rechauffe", false);
                loupGO.GetComponent<Move>().animShadow.SetBool("Rechauffe", false);
                faonGO.GetComponent<Move>().animUnlit.SetBool("Rechauffe", false);
                faonGO.GetComponent<Move>().animShadow.SetBool("Rechauffe", false);

                loupGO.GetComponent<Move>().isRechauffe = false;
                faonGO.GetComponent<Move>().isRechauffe = false;

                cold += Time.deltaTime * 10f;
                cold = Mathf.Clamp(cold, 0f, 10f);

            }
        }

        //if (cold < 8f && cold > 7f)
        //{
        //    loupGO.GetComponent<Move>().isGrelotte = true;
        //    faonGO.GetComponent<Move>().isGrelotte = true;
        //}
        //else if (cold < 5f && cold > 3f)
        //{
        //    loupGO.GetComponent<Move>().isGrelotte = true;
        //    faonGO.GetComponent<Move>().isGrelotte = true;
        //}
        //else if (cold < 0f)
        //{
        //    loupGO.GetComponent<Move>().isGrelotte = true;
        //    faonGO.GetComponent<Move>().isGrelotte = true;
        //}
        //else
        //{
        //    loupGO.GetComponent<Move>().isGrelotte = false;
        //    faonGO.GetComponent<Move>().isGrelotte = false;
        //}

    }
}
