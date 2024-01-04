using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public GameObject point;
    public GameObject spot;
    void Start()
    {
        point.SetActive(true);
        spot.SetActive(false);
        RenderSettings.fog = true;
        RenderSettings.ambientLight = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            point.SetActive(true);
            spot.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            point.SetActive(false);
            spot.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (RenderSettings.fog)
            {
                RenderSettings.fog = false;
                RenderSettings.ambientLight = Color.white;
            }
            else
            {
                RenderSettings.fog = true;
                RenderSettings.ambientLight = Color.black;
            }
        }
    }
}
