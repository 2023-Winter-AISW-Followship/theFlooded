using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharController_Motor : MonoBehaviour
{

    private float speed = 5.0f;
    private float walkSpeed = 5.0f;
    private float runSpeed = 5.0f;
    private float sitSpeed = 2.5f;
    private float sensitivity = 400.0f;
    private float WaterHeight = 15.5f;
    CharacterController character;
    public GameObject cam;
    float moveFB, moveLR, moveUD = 0f;
    float rotX, rotY;
    public bool webGLRightClickRotation = false;

    public AudioClip walkSound;


    void Start()
    {
        //LockCursor ();
        character = GetComponent<CharacterController>();
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;
            sensitivity = sensitivity * 1.5f;
        }
    }

    void Update()
    {
        moveControll();

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        Vector3 movement = new Vector3(moveLR, moveUD, moveFB);


        CameraRotation(cam, rotX, rotY);


        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);
        Debug.Log(moveUD);
    }

    void moveControll()
    {
        // sit
        if (Input.GetKeyDown(KeySetting.key[KeyAction.SIT]))
        {
            speed -= sitSpeed;
        }
        else if (Input.GetKeyUp(KeySetting.key[KeyAction.SIT]))
        {
            speed += sitSpeed;
        }

        // run
        if (Input.GetKeyDown(KeySetting.key[KeyAction.RUN]))
        {
            speed += runSpeed;
        }
        else if (Input.GetKeyUp(KeySetting.key[KeyAction.RUN]))
        {
            speed -= runSpeed;
        }

        // left, right
        if (Input.GetKey(KeySetting.key[KeyAction.LEFT]))
        {
            moveLR = -speed;
        }
        else if (Input.GetKey(KeySetting.key[KeyAction.RIGHT]))
        {
            moveLR = speed;
        }
        else
        {
            moveLR = 0f;
        }

        // front, back
        if (Input.GetKey(KeySetting.key[KeyAction.UP]))
        {
            moveFB = speed;
        }
        else if (Input.GetKey(KeySetting.key[KeyAction.DOWN]))
        {
            moveFB = -speed;
        }
        else
        {
            moveFB = 0f;
        }

        // jump
        if (character.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                moveUD = 6f;
            }
        }
        else
        {
            moveUD += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            moveUD = 0;
        }
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);
    }
}
