using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; //회전속도
    private float mouseX = 0f, mouseY = 0f;

    // Update is called once per frame
    void Update()
    {
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;

        mouseY = Mathf.Clamp(mouseY, -50f, 30f);
    }

    private void FixedUpdate()
    {
        transform.parent.localEulerAngles = new Vector3(0, mouseX, 0);
        transform.localEulerAngles = new Vector3(-mouseY, 0, 0);
    }
}