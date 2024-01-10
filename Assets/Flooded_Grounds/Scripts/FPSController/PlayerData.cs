using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public class Components
    {
        public Transform root;
        public Transform cam;

        public CapsuleCollider capsule;
        public Rigidbody rBody;
        public Animator anim;

        public AudioClip walkSound;
    }

    public class CheckOption
    {
        [Tooltip("지면으로 체크할 레이어 설정")]
        public LayerMask groundLayerMask = ~(1<<4 | 1<<6);

        [Range(0.01f, 0.5f), Tooltip("전방 감지 거리")]
        public float forwardCheckDistance = 1.7f;

        [Range(0.1f, 10.0f), Tooltip("지면 감지 거리")]
        public float groundCheckDistance = 3.5f;

        [Range(0.0f, 0.1f), Tooltip("지면 인식 허용 거리")]
        public float groundCheckThreshold = 0.01f;
    }

    public class MovementOption
    {
        [Range(1f, 10f), Tooltip("이동속도")]
        public float speed = 9f;

        [Range(1f, 3f), Tooltip("달리기 이동속도 증가 계수")]
        public float runningCoef = 2f;

        [Range(1f, 3f), Tooltip("앉기 이동속도 감소 계수")]
        public float sittingCoef = 0.3f;

        [Range(1f, 10f), Tooltip("점프 강도")]
        public float jumpForce = 4.5f;

        [Range(0f, 4f), Tooltip("경사로 이동속도 변화율(가속/감속)")]
        public float slopeAccel = 1f;

        [Range(1f, 70f), Tooltip("등반 가능한 경사각")]
        public float maxSlopeAngle = 45f;

        [Range(0f, 2f), Tooltip("앉을 때 yPos 감소")]
        public float sittingPos = 0.5f;

        [Range(-9.81f, 0f), Tooltip("중력")]
        public float gravity = -9.81f;
    }

    public class CurrentState
    {
        public bool isMoving;
        public bool isSitting;
        public bool isRunning;
        public bool isGrounded { get; set; }
        public bool isOnSteepSlope;   // 등반 불가능한 경사로에 올라와 있음
        public bool isJumping;
        public bool isForwardBlocked; // 전방에 장애물 존재
        public bool isOutOfControl;
        public bool isWater;
    }

    public class CurrentValue
    {
        public Vector3 worldMoveDir;
        public Vector3 groundNormal;
        public Vector3 groundCross;
        public Vector3 horizontalVelocity;

        [Space]
        public float groundDistance;
        public float groundSlopeAngle;         // 현재 바닥의 경사각
        public float groundVerticalSlopeAngle; // 수직으로 재측정한 경사각
        public float forwardSlopeAngle; // 캐릭터가 바라보는 방향의 경사각
        public float slopeAccel;        // 경사로 인한 가속/감속 비율

        [Space]
        public float gravity; // 직접 제어하는 중력값
    }

    private PlayerData data = null;
    private Components _component;
    private CheckOption _check;
    private MovementOption _movement;
    private CurrentState _state;
    private CurrentValue _value;

    public float _castRadius;
    public Vector3 CapsuleTopCenterPoint
       => new Vector3(transform.position.x, transform.position.y + (Com.capsule.height / 2) - Com.capsule.radius, transform.position.z);
    public Vector3 CapsuleBottomCenterPoint
        => new Vector3(transform.position.x, transform.position.y - Com.capsule.height / 2, transform.position.z);
    public Vector3 moveDestination;
    public Vector3 _moveDir;
    public Vector3 _worldMove;
    public float _capsuleRadiusDiff;
    public float moveFB, moveLR;

    private void Awake()
    {
        if(data == null)
        {
            data = this;
            data._component = new Components();
            data._check = new CheckOption();
            data._movement = new MovementOption();
            data._state = new CurrentState();
            data._value = new CurrentValue();

            data._component.capsule = gameObject.GetComponent<CapsuleCollider>();
            data._component.rBody = gameObject.GetComponent<Rigidbody>();
            data._component.cam = Camera.main.gameObject.transform;
            data._component.root = transform;

            _castRadius = Com.capsule.radius * 0.9f;
            _capsuleRadiusDiff = Com.capsule.radius - _castRadius + 0.05f;

            data._check.groundCheckDistance = Com.capsule.radius;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Components Com { get { if (data == null) { return null; } return _component; } set { _component = value; } }
    public CheckOption Check { get { if (data == null) { return null; } return _check; } set { _check = value; } }
    public MovementOption Movement { get { if (data == null) { return null; } return _movement; } set { _movement = value; } }
    public CurrentState State { get { if (data == null) { return null; } return _state; } set { _state = value; } }
    public CurrentValue Value { get { if (data == null) { return null; } return _value ; } set { _value = value; } }
}