using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMessage : MonoBehaviour
{
    GameObject cam;
    bool isDetected;

    private void Start()
    {
        cam = Camera.main.gameObject;
        isDetected = false;
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isDetected)
        {
            this.gameObject.SetActive(false);
        }
        transform.parent.LookAt(cam.transform);
        transform.parent.Rotate(0, 180, 0);
        isDetected = false;
    }

    public void detected()
    {
        isDetected = true;
        this.gameObject.SetActive(true);
    }
}
