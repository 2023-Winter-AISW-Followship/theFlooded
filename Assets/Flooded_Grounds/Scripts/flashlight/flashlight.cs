using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
    // Update is called once per frame
    Light flash;
    private void Start()
    {
        flash = GetComponent<Light>();
        flash.enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (flash.enabled)
            {
                flash.enabled = false;
            }
            else
            {
                flash.enabled = true;
            }
        }
    }
}
