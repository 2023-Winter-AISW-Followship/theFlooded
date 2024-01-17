using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Net;
#endif

public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;

    #region Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;
    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    private bool isWalking = false;

    #region Sprint

    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 7f;
    public float sprintDuration = 5f;
    public float sprintCooldown = .5f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    public bool useSprintBar = true;
    public bool hideBarWhenFull = true;
    public Image sprintBarBG;
    public Image sprintBar;
    public float sprintBarWidthPercent = .3f;
    public float sprintBarHeightPercent = .015f;

    private CanvasGroup sprintBarCG;
    private bool isSprinting = false;
    private float sprintRemaining;
    private float sprintBarWidth;
    private float sprintBarHeight;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;

    #endregion

    #region Jump

    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    private bool isGrounded = false;

    #endregion

    #region Crouch

    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    private bool isSitting = false;
    private Vector3 originalScale;

    #endregion
    #endregion

    #region Head Bob

    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        crosshairObject = GetComponentInChildren<Image>();

        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;
        jointOriginalPos = joint.localPosition;
        sprintRemaining = sprintDuration;
        sprintCooldownReset = sprintCooldown;
    }

    void Start()
    {
        crosshairObject.sprite = crosshairImage;
        crosshairObject.color = crosshairColor;

        #region Sprint Bar

        sprintBarCG = GetComponentInChildren<CanvasGroup>();

        if(useSprintBar)
        {
            sprintBarBG.gameObject.SetActive(true);
            sprintBar.gameObject.SetActive(true);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            sprintBarWidth = screenWidth * sprintBarWidthPercent;
            sprintBarHeight = screenHeight * sprintBarHeightPercent;

            sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
            sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);

            if(hideBarWhenFull)
            {
                sprintBarCG.alpha = 0;
            }
        }
        else
        {
            sprintBarBG.gameObject.SetActive(false);
            sprintBar.gameObject.SetActive(false);
        }

        #endregion
    }

    private void Update()
    {
        cameraCanMove = !Pause.GameIsPaused;
        playerCanMove = !Pause.GameIsPaused;

        #region Camera

        if(cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");

            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.parent.localEulerAngles = new Vector3(pitch, 0, 0);
        }
        #endregion

        #region Sprint

        if(isSprinting)
        {
            sprintRemaining -= 1 * Time.deltaTime;
            if (sprintRemaining <= 0)
            {
                isSprinting = false;
                isSprintCooldown = true;
            }
        }
        else
        {
            sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
        }

        if(isSprintCooldown)
        {
            sprintCooldown -= 1 * Time.deltaTime;
            if (sprintCooldown <= 0)
            {
                isSprintCooldown = false;
            }
        }
        else
        {
            sprintCooldown = sprintCooldownReset;
        }

        if(useSprintBar)
        {
            float sprintRemainingPercent = sprintRemaining / sprintDuration;
            sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);
        }

        #endregion

        #region Jump

        if(Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        #endregion

        #region Crouch
            
        if(Input.GetKeyDown(crouchKey))
        {
            Sit();
        }

        #endregion

        CheckGround();

        if(enableHeadBob)
        {
            HeadBob();
        }
    }

    void FixedUpdate()
    {
        #region Movement

        if (playerCanMove)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            if (Input.GetKey(sprintKey) && sprintRemaining > 0f && !isSprintCooldown)
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                if (velocityChange.x != 0 || velocityChange.z != 0)
                {
                    isSprinting = true;

                    if (isSitting)
                    {
                        Sit();
                    }

                    if (hideBarWhenFull)
                    {
                        sprintBarCG.alpha += 5 * Time.deltaTime;
                    }
                }

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            else
            {
                isSprinting = false;

                if (hideBarWhenFull && sprintRemaining == sprintDuration)
                {
                    sprintBarCG.alpha -= 3 * Time.deltaTime;
                }

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }

        #endregion
    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;
        }

        if(isSitting)
        {
            Sit();
        }
    }

    private void Sit()
    {
        isSitting = !isSitting;
        if (isSitting)
        {
            Camera.main.transform.position -= Vector3.up * 0.5f;
            walkSpeed *= speedReduction;
        }
        else
        {
            Camera.main.transform.position += Vector3.up * 0.5f;
            walkSpeed /= speedReduction;
        }
    }

    private void HeadBob()
    {
        if(isWalking)
        {
            if(isSprinting)
            {
                timer += Time.deltaTime * (bobSpeed + sprintSpeed);
            }
            else if (isSitting)
            {
                timer += Time.deltaTime * (bobSpeed * speedReduction);
            }
            else
            {
                timer += Time.deltaTime * bobSpeed;
            }
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }
}