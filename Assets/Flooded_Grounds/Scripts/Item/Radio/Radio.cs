using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Radio : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    public float raycastDistance = 5f;
    GameObject point;

    void Start()
    {
        point = GameObject.Find("GameObject").transform.GetChild(0).gameObject;
    }

    void Update()
    {
        point.SetActive(false);
        if (transform.parent == Camera.main.transform)
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out hit, raycastDistance)) //인식할 수 있는 범위 안에서 물체 확인
            {
                if (hit.collider.gameObject.layer == 3)
                {
                    point.transform.position = hit.point;
                    point.SetActive(true);
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                transform.parent = null;
                transform.position = hit.point;
                transform.rotation = point.transform.rotation;
                GetComponentInChildren<AudioSource>().Play();
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Collider>().enabled = true;
            }
        }
    }
}
