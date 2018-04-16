using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    [Header("Test")]
    public bool playTest = true;

    [Space]
    [Header("Players")]
    public GameObject loupGO;
    public GameObject faonGO;

    [Space]
    [Header("Camera")]
    public float camHeightMin;
    public float camHeightMax;
    public float camHeightAdd;
    public float distPersos;
    public GameObject plane;

    [Space]
    [Header("Gestion Froid")]
    public bool freezable;
    public float distCold = 0.6f;
    public float cold = 10f;
    public float timeColdOrigin = 10f;

    [Space]
    [Header("Rapace")]
    public GameObject rapace;
    public bool isRapace;

    void Start () {
        freezable = false;
	}

    void Update()
    {
        if (isRapace)
        {
            rapace.GetComponent<RapaceBehavior>();
        }


        if (plane != null)
        {
            plane.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        
        
        //CAMERA
        var newPositionCam = faonGO.transform.position + (loupGO.transform.position - faonGO.transform.position) / 2;
        distPersos = Vector3.Distance(loupGO.transform.position, faonGO.transform.position);

        if (freezable)
        {
            transform.position = new Vector3(newPositionCam.x, Mathf.Clamp(distPersos / 3, camHeightMin, camHeightMax) + camHeightAdd, newPositionCam.z - Mathf.Clamp(distPersos / 3, camHeightMin, camHeightMax) - camHeightAdd);

            //COLD
            if (distPersos > distCold)
            {
                cold -= Time.deltaTime * distPersos / 5;
            }
            else
            {
                if (loupGO.GetComponent<PlayerControl>().isWalking == false && faonGO.GetComponent<PlayerControl>().isWalking == false)
                {
                    loupGO.GetComponent<PlayerControl>().animUnlit.SetBool("Rechauffe", true);
                    faonGO.GetComponent<PlayerControl>().animUnlit.SetBool("Rechauffe", true);

                    loupGO.GetComponent<PlayerControl>().isRechauffe = true;
                    faonGO.GetComponent<PlayerControl>().isRechauffe = true;

                    cold = timeColdOrigin;
                }
                else
                {
                    loupGO.GetComponent<PlayerControl>().animUnlit.SetBool("Rechauffe", false);
                    faonGO.GetComponent<PlayerControl>().animUnlit.SetBool("Rechauffe", false);

                    loupGO.GetComponent<PlayerControl>().isRechauffe = false;
                    faonGO.GetComponent<PlayerControl>().isRechauffe = false;

                    cold += Time.deltaTime * timeColdOrigin;

                    cold = Mathf.Clamp(cold, -1f, timeColdOrigin);
                }
            }

            if (cold < 0f)
            {
                loupGO.transform.position = new Vector3(0f, 0f, 0f);
                faonGO.transform.position = new Vector3(0f, 0f, 0f);
                cold = timeColdOrigin;
            }

            if (cold < 8f && cold > 7f)
            {
                loupGO.GetComponent<PlayerControl>().isGrelotte = true;
                faonGO.GetComponent<PlayerControl>().isGrelotte = true;
            }
            else if (cold < 5f && cold > 3f)
            {
                loupGO.GetComponent<PlayerControl>().isGrelotte = true;
                faonGO.GetComponent<PlayerControl>().isGrelotte = true;
            }
            else if (cold < 0f)
            {
                loupGO.GetComponent<PlayerControl>().isGrelotte = true;
                faonGO.GetComponent<PlayerControl>().isGrelotte = true;
            }
            else
            {
                loupGO.GetComponent<PlayerControl>().isGrelotte = false;
                faonGO.GetComponent<PlayerControl>().isGrelotte = false;
            }
        }
        else
        {
            //CAMERA
            transform.position = new Vector3(loupGO.transform.position.x, loupGO.transform.position.y + camHeightAdd, loupGO.transform.position.z - camHeightAdd) ;

            if (distPersos < 0.6f)
            {
                freezable = true;
                faonGO.GetComponent<PlayerControl>().isFreeze = false;
            }
        }
    }
}
