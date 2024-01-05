using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CharController_Motor : MonoBehaviour
{
    public GameObject cam;
    public AudioClip walkSound;
    
    private float walkSpeed = 6.0f;
    private float runSpeed = 6.0f;
    private float sitSpeed = 3f;
    
    private float speed;
    private float sensitivity = 8f;
    CharacterController character;
    float moveFB, moveLR, moveUD = 0f;
    float rotX, rotY;

    void Start()
    {
        character = GetComponent<CharacterController>();
        speed = walkSpeed;
    }

    void Update()
    {
        moveControll();

        rotX += Input.GetAxis("Mouse X") * sensitivity;
        this.transform.localEulerAngles = new Vector3(0, rotX, 0);

        Vector3 movement = new Vector3(moveLR, moveUD, moveFB);

        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);
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
                moveUD = 5f;
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
}
