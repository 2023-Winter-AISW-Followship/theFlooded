using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharController_Moter : MonoBehaviour
{
    private Rigidbody rb;
    public static Transform player;

    #region Sound Variables

    float step;
    float pitch;
    float volume;

    #endregion

    #region Camera Movement Variables

    public Camera playerCamera;

    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    private Image crosshairObject;
    #endregion

    #region Movement Variables

    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    private bool isWalking = false;
    private bool isWater = false;

    private const int waterHeight = 9;
    int moveLR, moveFB;

    #region Sprint

    public float sprintSpeed = 10f;
    public float sprintDuration = 20f;
    public float sprintRegen = 50f;
    public float sprintCooldown = 5f;

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

    public float jumpPower = 5f;

    private bool isGrounded = false;

    #endregion

    #region Sit

    public float sitHeight = .75f;
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
        player = transform;
        rb = GetComponent<Rigidbody>();

        crosshairObject = GetComponentInChildren<Image>();

        originalScale = transform.localScale;
        jointOriginalPos = joint.localPosition;
        sprintRemaining = sprintDuration;
        sprintCooldownReset = sprintCooldown;
    }

    void Start()
    {
        crosshairObject.sprite = crosshairImage;
        crosshairObject.color = crosshairColor;

        StartCoroutine(Step());

        #region Sprint Bar

        sprintBarCG = GetComponentInChildren<CanvasGroup>();

        sprintBarBG.gameObject.SetActive(true);
        sprintBar.gameObject.SetActive(true);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        sprintBarWidth = screenWidth * sprintBarWidthPercent;
        sprintBarHeight = screenHeight * sprintBarHeightPercent;

        sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
        sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);

        if (hideBarWhenFull)
        {
            sprintBarCG.alpha = 0;
        }

        #endregion
    }

    IEnumerator Step()
    {
        int i = 0;
        while (true)
        {
            if (isWalking)
            {
                if (isSprinting)
                {
                    step = sprintSpeed;
                }
                else
                {
                    step = walkSpeed;
                }

                step /= sprintSpeed;
                volume = step;
                pitch = Mathf.Lerp(1f, 0.25f, step);

                Sound.FootStep(i, transform.position, volume, Sound.Type.Player);

                yield return new WaitForSeconds(pitch);
                i = (i + 1) % 10;
            }
            yield return null;
        }
    }

    Vector3 targetVelocity;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (sprintSpeed == 10)
            {
                sprintSpeed = 20;
                sprintRegen = 1;
            }
            else
            {
                sprintSpeed = 10;
                sprintRegen = 50;
            }
        }

        #region Camera

        if (!Pause.GameIsPaused)
        {
            mouseX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY -= mouseSensitivity * Input.GetAxis("Mouse Y");

            mouseY = Mathf.Clamp(mouseY, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, mouseX, 0);
            playerCamera.transform.localEulerAngles = new Vector3(mouseY, 0, 0);
        }
        #endregion

        #region Sprint

        if (isSprinting)
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
            sprintRemaining = Mathf.Clamp(sprintRemaining += (sprintDuration / sprintRegen) * Time.deltaTime, 0, sprintDuration);
        }

        if (isSprintCooldown)
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

        float sprintRemainingPercent = sprintRemaining / sprintDuration;
        sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);

        #endregion

        #region Jump

        if (Input.GetKeyDown(KeySetting.key[KeyAction.JUMP]) && isGrounded)
        {
            Jump();
        }

        #endregion

        #region Sit

        if (Input.GetKeyDown(KeySetting.key[KeyAction.SIT]))
        {
            Sit();
        }

        #endregion

        CheckGround();

        if (enableHeadBob)
        {
            HeadBob();
        }
    }

    private void FixedUpdate()
    {
        #region Movement

        if (!Pause.GameIsPaused)
        {
            moveLR = 0;
            moveFB = 0;

            if (Input.GetKey(KeySetting.key[KeyAction.LEFT])) moveLR -= 1;
            if (Input.GetKey(KeySetting.key[KeyAction.RIGHT])) moveLR += 1;

            if (Input.GetKey(KeySetting.key[KeyAction.FORWARD])) moveFB += 1;
            if (Input.GetKey(KeySetting.key[KeyAction.BACK])) moveFB -= 1;

            targetVelocity = new Vector3(moveLR, 0, moveFB).normalized;

            isWalking = (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded) ? true : false;

            if (isWalking && Input.GetKey(KeySetting.key[KeyAction.RUN]) && sprintRemaining > 0f && !isSprintCooldown)
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;
                
                if(targetVelocity.magnitude > 0.01f)
                {
                    isSprinting = true;
                }

                if (isSitting)
                {
                    Sit();
                }

                if (hideBarWhenFull)
                {
                    sprintBarCG.alpha += 5 * Time.deltaTime;
                }

            }
            else
            {
                isSprinting = false;

                if (hideBarWhenFull && sprintRemaining == sprintDuration)
                {
                    sprintBarCG.alpha -= 3 * Time.deltaTime;
                }

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
            }

            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -10f, 10f);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -10f, 10f);
            velocityChange.y = 0;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        #endregion
    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance)) isGrounded = true;
        else isGrounded = false;

        isWater = transform.position.y <= waterHeight;
        if (isWater) transform.position = new Vector3(transform.position.x, waterHeight, transform.position.z);

    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;
        }

        if (isSitting)
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
        if (isWalking)
        {
            if (isSprinting)
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


#if UNITY_EDITOR
[CustomEditor(typeof(CharController_Moter)), InitializeOnLoadAttribute]
public class CharController_MoterEditor : Editor
{
    CharController_Moter fpc;
    SerializedObject SerFPC;

    private void OnEnable()
    {
        fpc = (CharController_Moter)target;
        SerFPC = new SerializedObject(fpc);
    }

    public override void OnInspectorGUI()
    {
        SerFPC.Update();

        EditorGUILayout.Space();
        GUILayout.Label("Modular First Person Controller", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        GUILayout.Label("By Jess Case", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        GUILayout.Label("version 1.0.1", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        EditorGUILayout.Space();

        #region Camera Setup

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Camera Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        fpc.playerCamera = (Camera)EditorGUILayout.ObjectField(new GUIContent("Camera", "Camera attached to the controller."), fpc.playerCamera, typeof(Camera), true);

        fpc.mouseSensitivity = EditorGUILayout.Slider(new GUIContent("Look Sensitivity", "Determines how sensitive the mouse movement is."), fpc.mouseSensitivity, .1f, 10f);
        fpc.maxLookAngle = EditorGUILayout.Slider(new GUIContent("Max Look Angle", "Determines the max and min angle the player camera is able to look."), fpc.maxLookAngle, 40, 90);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Crosshair Image", "Sprite to use as the crosshair."));
        fpc.crosshairImage = (Sprite)EditorGUILayout.ObjectField(fpc.crosshairImage, typeof(Sprite), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        fpc.crosshairColor = EditorGUILayout.ColorField(new GUIContent("Crosshair Color", "Determines the color of the crosshair."), fpc.crosshairColor);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        #endregion

        #region Movement Setup

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Movement Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        fpc.walkSpeed = EditorGUILayout.Slider(new GUIContent("Walk Speed", "Determines how fast the player will move while walking."), fpc.walkSpeed, .1f, fpc.sprintSpeed);

        EditorGUILayout.Space();

        #region Sprint

        GUILayout.Label("Sprint", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));

        fpc.sprintSpeed = EditorGUILayout.Slider(new GUIContent("Sprint Speed", "Determines how fast the player will move while sprinting."), fpc.sprintSpeed, fpc.walkSpeed, 20f);

        fpc.sprintDuration = EditorGUILayout.Slider(new GUIContent("Sprint Duration", "Determines how long the player can sprint"), fpc.sprintDuration, 1f, 20f);
        fpc.sprintRegen = EditorGUILayout.Slider(new GUIContent("Sprint Regen", "Determines how long sprint duration regen"), fpc.sprintRegen, 1f, fpc.sprintDuration * 3f);
        fpc.sprintCooldown = EditorGUILayout.Slider(new GUIContent("Sprint Cooldown", "Determines how long the recovery time is when the player runs out of sprint."), fpc.sprintCooldown, .1f, fpc.sprintDuration);

        EditorGUILayout.BeginHorizontal();
        fpc.hideBarWhenFull = EditorGUILayout.ToggleLeft(new GUIContent("Hide Full Bar", "Hides the sprint bar when sprint duration is full, and fades the bar in when sprinting. Disabling this will leave the bar on screen at all times when the sprint bar is enabled."), fpc.hideBarWhenFull);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Bar BG", "Object to be used as sprint bar background."));
        fpc.sprintBarBG = (Image)EditorGUILayout.ObjectField(fpc.sprintBarBG, typeof(Image), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Bar", "Object to be used as sprint bar foreground."));
        fpc.sprintBar = (Image)EditorGUILayout.ObjectField(fpc.sprintBar, typeof(Image), true);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        fpc.sprintBarWidthPercent = EditorGUILayout.Slider(new GUIContent("Bar Width", "Determines the width of the sprint bar."), fpc.sprintBarWidthPercent, .1f, .5f);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        fpc.sprintBarHeightPercent = EditorGUILayout.Slider(new GUIContent("Bar Height", "Determines the height of the sprint bar."), fpc.sprintBarHeightPercent, .001f, .025f);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        #endregion

        #region Jump

        GUILayout.Label("Jump", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));

        fpc.jumpPower = EditorGUILayout.Slider(new GUIContent("Jump Power", "Determines how high the player will jump."), fpc.jumpPower, .1f, 20f);

        EditorGUILayout.Space();

        #endregion

        #region Sit

        GUILayout.Label("Sit", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));

        fpc.sitHeight = EditorGUILayout.Slider(new GUIContent("Sit Height", "Determines the y scale of the player object when sitted."), fpc.sitHeight, .1f, 1);
        fpc.speedReduction = EditorGUILayout.Slider(new GUIContent("Speed Reduction", "Determines the percent 'Walk Speed' is reduced by. 1 being no reduction, and .5 being half."), fpc.speedReduction, .1f, 1);

        #endregion

        #endregion

        #region Head Bob

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Head Bob Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        fpc.enableHeadBob = EditorGUILayout.ToggleLeft(new GUIContent("Enable Head Bob", "Determines if the camera will bob while the player is walking."), fpc.enableHeadBob);


        GUI.enabled = fpc.enableHeadBob;
        fpc.joint = (Transform)EditorGUILayout.ObjectField(new GUIContent("Camera Joint", "Joint object position is moved while head bob is active."), fpc.joint, typeof(Transform), true);
        fpc.bobSpeed = EditorGUILayout.Slider(new GUIContent("Speed", "Determines how often a bob rotation is completed."), fpc.bobSpeed, 1, 20);
        fpc.bobAmount = EditorGUILayout.Vector3Field(new GUIContent("Bob Amount", "Determines the amount the joint moves in both directions on every axes."), fpc.bobAmount);
        GUI.enabled = true;

        #endregion

        if (GUI.changed)
        {
            EditorUtility.SetDirty(fpc);
            Undo.RecordObject(fpc, "FPC Change");
            SerFPC.ApplyModifiedProperties();
        }
    }

}

#endif