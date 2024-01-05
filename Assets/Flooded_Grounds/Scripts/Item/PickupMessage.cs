using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMessage : MonoBehaviour
{
    GameObject cam;

    Vector3 startScale;
    public float distance = 5f;

    private void Start()
    {
        cam = Camera.main.gameObject;
        startScale = transform.localScale;
    }

    void Update()
    {
        float dist = Vector3.Distance(cam.transform.position, transform.position);
        Vector3 newScale = startScale * dist / distance;
        transform.localScale = newScale;

        transform.parent.rotation = cam.transform.rotation;
    }
}
