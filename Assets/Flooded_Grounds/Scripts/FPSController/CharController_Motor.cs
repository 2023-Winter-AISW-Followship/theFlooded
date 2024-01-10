using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class CharController_Motor : PlayerData
{
    void Start()
    {
        //이동
        Observable.EveryUpdate()
            .Where(_ => !Pause.GameIsPaused)
            .Subscribe(_ =>
            {
                CheckGround();
                CheckForward();
                Move();
            })
            .AddTo(gameObject);

        // 키보드 입력
        this.UpdateAsObservable()
            .Where(_ => !Pause.GameIsPaused 
                && State.isGrounded)
            .Subscribe(_ => InputKey());
    }

    void InputKey()
    {
        moveFB = 0f;
        moveLR = 0f;

        // 점프
        if (Input.GetKey(KeySetting.key[KeyAction.JUMP]))
        {
            Jump();
        }

        State.isRunning = false;
        // 달리기
        if (Input.GetKey(KeySetting.key[KeyAction.RUN]))
        {
            Run();
        }

        // 앉기
        if (Input.GetKeyDown(KeySetting.key[KeyAction.SIT]))
        {
            Sit();
        }

        // left, right
        if (Input.GetKey(KeySetting.key[KeyAction.LEFT])) moveLR -= 1f;
        if (Input.GetKey(KeySetting.key[KeyAction.RIGHT])) moveLR += 1f;

        // front, back
        if (Input.GetKey(KeySetting.key[KeyAction.UP])) moveFB += 1f;
        if (Input.GetKey(KeySetting.key[KeyAction.DOWN])) moveFB -= 1f;

        moveDestination = new Vector3(moveLR, 0, moveFB).normalized;
        _moveDir = Vector3.Lerp(_moveDir, moveDestination, 4f * Time.deltaTime);
        Value.worldMoveDir = Com.root.TransformDirection(_moveDir);

        State.isMoving = _moveDir.sqrMagnitude > 0.01f;

        if (!State.isMoving)
        {
            Value.worldMoveDir = Vector3.zero;
            State.isRunning = false;
        }

        CalMove();
    }

    void CalMove()
    {
        if(State.isForwardBlocked && !State.isGrounded || State.isJumping && State.isGrounded)
        {
            Value.horizontalVelocity = Vector3.zero;
            _moveDir = Vector3.zero;
        }
        else
        {
            Value.horizontalVelocity = Value.worldMoveDir
                * (!State.isMoving ? 0 : Movement.speed
                    * (State.isRunning ? Movement.runningCoef : 1)
                    * (State.isSitting ? Movement.sittingCoef : 1));
        }

        if (State.isGrounded || Value.groundDistance < Check.groundCheckDistance && !State.isJumping)
        {
            if (State.isMoving && !State.isForwardBlocked)
            {
                // 경사로 인한 가속/감속
                if (Movement.slopeAccel > 0f)
                {
                    bool isPlus = Value.forwardSlopeAngle >= 0f;
                    float absFsAngle = isPlus ? Value.forwardSlopeAngle : -Value.forwardSlopeAngle;
                    float accel = Movement.slopeAccel * absFsAngle * 0.01111f + 1f;
                    Value.slopeAccel = !isPlus ? accel : 1.0f / accel;

                    Value.horizontalVelocity *= Value.slopeAccel;
                }

                // 벡터 회전 (경사로)
                Value.horizontalVelocity =
                    Quaternion.AngleAxis(-Value.groundSlopeAngle, Value.groundCross) * Value.horizontalVelocity;
            }
        }
    }

    void Jump()
    {
        Value.gravity = Movement.jumpForce;
        State.isJumping = true;
    }

    void Sit()
    {
        State.isSitting = !State.isSitting;
    }

    void Run()
    {
        State.isRunning = true;
    }

    void Move()
    {
        if (State.isGrounded)
        {
            Value.gravity = 0f;
            State.isJumping = false;
        }
        else
        {
            Value.gravity += Time.deltaTime * Movement.gravity;
        }

        Com.rBody.velocity = Value.horizontalVelocity + Vector3.up * Value.gravity;
    }

    void CheckGround()
    {
        Value.groundDistance = float.MaxValue;
        Value.groundNormal = Vector3.up;
        Value.groundSlopeAngle = 0f;
        Value.forwardSlopeAngle = 0f;

        bool cast = Physics.SphereCast(
            CapsuleBottomCenterPoint,
            _castRadius,
            -transform.up,
            out var hit,
            Check.groundCheckDistance,
            Check.groundLayerMask,
            QueryTriggerInteraction.Ignore);

        State.isGrounded = false;

        if (cast)
        {
            // 지면 노멀벡터 초기화
            Value.groundNormal = hit.normal;

            // 현재 위치한 지면의 경사각 구하기(캐릭터 이동방향 고려)
            Value.groundSlopeAngle = Vector3.Angle(Value.groundNormal, Vector3.up);
            Value.forwardSlopeAngle = Vector3.Angle(Value.groundNormal, Value.worldMoveDir) - 90f;

            State.isOnSteepSlope = Value.groundSlopeAngle >= Movement.maxSlopeAngle;

            Value.groundDistance = Mathf.Max(hit.distance - _capsuleRadiusDiff - Check.groundCheckThreshold, 0f);

            State.isGrounded =
                (Value.groundDistance <= 0.0001f) && !State.isOnSteepSlope;

            _gzGroundTouch = hit.point;
        }

        // 월드 이동벡터 회전축
        Value.groundCross = Vector3.Cross(Value.groundNormal, Vector3.up);
    }

    void CheckForward()
    {
        bool cast = Physics.CapsuleCast(
            CapsuleBottomCenterPoint,
            CapsuleTopCenterPoint,
            _castRadius,
            transform.forward,
            out var hit,
            Check.forwardCheckDistance,
            -1,
            QueryTriggerInteraction.Ignore);

        State.isForwardBlocked = false;
        if (cast)
        {
            float forwardObstacleAngle = Vector3.Angle(hit.normal, Vector3.up);
            State.isForwardBlocked = forwardObstacleAngle >= Movement.maxSlopeAngle;

            _gzForwardTouch = hit.point;
        }

        Debug.Log(State.isForwardBlocked);
    }


    private Vector3 _gzGroundTouch;
    private Vector3 _gzForwardTouch;

    void OnDrawGizmos()
    {
        float _gizmoRadius = 0.05f;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_gzGroundTouch, _gizmoRadius);

        if (State.isForwardBlocked)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_gzForwardTouch, _gizmoRadius);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(CapsuleBottomCenterPoint, -transform.up * Check.groundCheckDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(CapsuleTopCenterPoint, transform.forward * Check.forwardCheckDistance);

        Gizmos.color = new Color(0.5f, 1.0f, 0.8f, 0.8f);
        Gizmos.DrawWireSphere(CapsuleTopCenterPoint, _castRadius);
        Gizmos.DrawWireSphere(CapsuleBottomCenterPoint, _castRadius);
    }
}
