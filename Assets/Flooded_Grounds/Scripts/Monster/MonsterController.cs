using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    #region Variable
    [SerializeField]
    private MonsterData monsterData;

    [SerializeField]
    private LayerMask soundLayer;
    [SerializeField]
    private LayerMask sparklerLayer;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask obstacleLayer = 1 << 0;

    Animator animator;
    NavMeshAgent agent;
    AudioSource audio;

    private float awakeTime = 10f;
    Vector3 destination;
    
    bool isFaint = false;
    bool isRecognize = false;
    bool isTarget = false;

    float step;
    float pitch;
    float volume;

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    Vector3 lookDir => AngleToDir(transform.eulerAngles.y);
    Collider[] targets = new Collider[5];
    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();

        agent.speed = monsterData.Speed;
        agent.angularSpeed = 360f;
        destination = agent.destination;

        this.UpdateAsObservable()
            .Where(_ => !isFaint)
            .Subscribe(_ =>
            {
                Transform target = SoundRecognize();
                Transform temp = SightRecognize();
                if(temp != null)
                {
                    target = temp;
                }
                if(target != null)
                {
                    isRecognize = true;
                    isTarget = true;
                    destination = target.position;
                }
                animator.SetBool("recognize", isRecognize);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("howl")) audio.Stop();
                else if (!audio.isPlaying) audio.Play();
            });

        this.UpdateAsObservable()
            .Where(_ => isRecognize)
            .Subscribe(_ => Attack());

        this.UpdateAsObservable()
            .Where(_ => isTarget)
            .Subscribe(_ => Move());

        StartCoroutine(Step());
    }

    #region Sound
    IEnumerator Step()
    {
        int i = 0;
        while (true)
        {
            if (isTarget)
            {
                if (isRecognize) step = monsterData.RunSpeed;
                else step = monsterData.Speed;

                step /= monsterData.RunSpeed;
                volume = step;
                pitch = Mathf.Lerp(1f, 0.25f, step);

                Sound.FootStep(i, transform.position, volume, Sound.Type.Monster);

                yield return new WaitForSeconds(pitch);
                i = (i + 1) % 10;
            }
            yield return null;
        }
    }

    void Howling()
    {
        Sound.HowlingSound(monsterData.HowlingSound, gameObject);
    }
    #endregion

    #region Movement
    void RandomTarget()
    {
        Vector3 randomTarget;
        do
        {
            randomTarget = UnityEngine.Random.insideUnitSphere * 20f + transform.position;
        } while (Vector3.Distance(randomTarget, transform.position) < 10f);
        if (NavMesh.SamplePosition(randomTarget, out NavMeshHit hit, 20f, NavMesh.AllAreas))
        {
            randomTarget = hit.position;

            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(randomTarget, path))
            {
                destination = randomTarget;
                isTarget = true;
            }
            else RandomTarget();
        }
        else RandomTarget();

        return;
    }

    void Move()
    {
        if(destination == null) return;

        if(isRecognize) agent.speed = monsterData.RunSpeed;
        else agent.speed = monsterData.Speed;

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(destination, path))
        {
            if(agent.speed == 0) Rotate();
            else
            {
                animator.SetBool("stop", false);
                agent.SetDestination(destination);
            }
        }

        if(!agent.pathPending
            &&
            agent.velocity.magnitude > 0.2f
            &&
            agent.remainingDistance < agent.stoppingDistance)
        {
            if (isRecognize) Rotate();
            else
            {
                isTarget = false;
                animator.SetTrigger("breath");
            }
        }
    }

    void Rotate()
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        if (Vector3.Distance(destination, CharController_Moter.player.position) < 0.1f
            ||
            Vector3.Distance(destination, transform.position) < monsterData.Reach)
        {
            animator.SetBool("stop", true);
            agent.speed = 0;
        }
        else
        {
            isRecognize = false;
        }
    }
    #endregion

    #region Recognize
    Transform SoundRecognize()
    {
        float max = -1;
        Transform target = null;

        int size = Physics.OverlapSphereNonAlloc(
            transform.position,
            monsterData.SoundRecognitionDist,
            targets,
            soundLayer);

        for(int i = 0; i < size; i++)
        {
            float targetDist = Vector3.Distance(transform.position, targets[i].transform.position);
            float distWeight = Mathf.Lerp(1.2f, 0, targetDist / monsterData.SoundRecognitionDist);
            float volumePerDist = targets[i].GetComponent<AudioSource>().volume * distWeight;
            if (volumePerDist > 0.25f && volumePerDist > max)
            {
                max = volumePerDist;
                target = targets[i].transform;
            }
        }
        return target;
    }

    Transform SightRecognize()
    {
        Transform target = null;

        int size = Physics.OverlapSphereNonAlloc(
            transform.position,
            monsterData.SightRecognitionDist,
            targets,
            playerLayer | sparklerLayer);

        for (int i = 0; i < size; i++)
        {
            
            bool isObstacle = Physics.Linecast(transform.position, targets[i].transform.position, obstacleLayer);

            if (monsterData.MonsterName.Equals("Gazer") && targets[i].gameObject.layer == Math.Log(sparklerLayer.value, 2))
            {
                Debug.Log(targets[i].name);
                Vector3 targetDir = (targets[i].transform.position - transform.position).normalized;
                float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
                if (targetAngle <= monsterData.SightRecognitionAngle * 0.5f
                    &&
                    !isObstacle)
                {
                    Debug.DrawLine(transform.position, targets[i].transform.position, Color.red);
                    target = targets[i].transform;
                    break;
                }
            }
            else if (targets[i].gameObject.layer == Math.Log(playerLayer.value, 2))
            {
                Vector3 targetDir = (targets[i].transform.position - transform.position).normalized;
                float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
                if (targetAngle <= monsterData.SightRecognitionAngle * 0.5f
                    &&
                    !isObstacle)
                {
                    Debug.DrawLine(transform.position, targets[i].transform.position, Color.red);
                    target = targets[i].transform;
                }
            }
        }

        return target;
    }
    #endregion

    #region Event
    void Attack()
    {

        bool inRange = Physics.CheckSphere(
            transform.position,
            monsterData.Reach,
            playerLayer);

        if (inRange)
        {
            animator.SetTrigger("inRange");
        }
    }

    void OnHit()
    {
        bool hit = Physics.CheckBox(
            transform.position,
            monsterData.HitSize / 2,
            transform.rotation,
            playerLayer);

        if (hit)
        {
            PlayerHPController.TakeDamage(monsterData.Damage);
            Debug.Log("Attacked!!");
        }
    }

    public void bottle()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("faint")
            ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("wakeUp"))
        {
            return;
        }
        isFaint = true;
        animator.SetTrigger("bottle");
        StartCoroutine(WakeUp());
    }

    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(awakeTime);
        isFaint = false;
        animator.SetTrigger("wakeUp");
    }
    #endregion

    private void OnDrawGizmos()
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(Vector3.zero, monsterData.SightRecognitionDist);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, monsterData.SoundRecognitionDist);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, monsterData.Reach);
        

        Vector3 rightDir = AngleToDir(Vector3.forward.y + monsterData.SightRecognitionAngle * 0.5f);
        Vector3 leftDir = AngleToDir(Vector3.forward.y - monsterData.SightRecognitionAngle * 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(Vector3.zero, rightDir * monsterData.SightRecognitionDist);
        Gizmos.DrawRay(Vector3.zero, leftDir * monsterData.SightRecognitionDist);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(Vector3.zero, Vector3.forward * monsterData.SightRecognitionDist);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, monsterData.HitSize);
    }
}
