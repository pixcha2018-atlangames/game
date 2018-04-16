using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastLight : MonoBehaviour {

    public int angleLaser;
    LineRenderer line;
    public GameObject loupGO;
    public GameObject faonGO;


    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
        transform.Rotate(0, angleLaser, 0);
    }

    void Update()
    {
        StopCoroutine("TestFaon");
        StartCoroutine("TestFaon");

        StopCoroutine("TestLoup");
        StartCoroutine("TestLoup");


    }

    IEnumerator TestLoup()
    {
        line.enabled = true;

        transform.LookAt(loupGO.transform.position);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        line.SetPosition(0, ray.origin);

        if (Physics.Raycast(ray, out hit, 100))
        {
            line.SetPosition(1, hit.point);

            if (hit.transform.CompareTag("Loup"))
            {
                loupGO.GetComponent<PlayerControl>().isLightened = true;
            }
            else
            {
                loupGO.GetComponent<PlayerControl>().isLightened = false;
            }
        }
        else
        {
            line.SetPosition(1, ray.GetPoint(100));
        }

        yield return null;

        line.enabled = false;
    }

    IEnumerator TestFaon()
    {
        line.enabled = true;

        transform.LookAt(faonGO.transform.position);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        line.SetPosition(0, ray.origin);

        if (Physics.Raycast(ray, out hit, 100))
        {
            line.SetPosition(1, hit.point);

            if (hit.transform.CompareTag("Faon"))
            {
                faonGO.GetComponent<PlayerControl>().isLightened = true;
            }
            else
            {
                faonGO.GetComponent<PlayerControl>().isLightened = false;

            }

        }
        else
        {
            line.SetPosition(1, ray.GetPoint(100));
        }

        yield return null;

        line.enabled = false;
    }
}
