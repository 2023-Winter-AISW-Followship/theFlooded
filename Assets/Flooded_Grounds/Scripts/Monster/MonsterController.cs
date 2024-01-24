using System;
using System.Collections;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterData monsterData;

    [SerializeField]
    private LayerMask soundLayer;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask obstacleLayer;

    Animator animator;
    NavMeshAgent agent;

    private float awakeTime = 10f;
    Vector3 destination;
    

    bool faint = false;
    bool recognize = false;

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    Vector3 lookDir => AngleToDir(transform.eulerAngles.y);
    Collider[] targets = new Collider[5];

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = monsterData.Speed;
        agent.angularSpeed = 360f;
        destination = agent.destination;

        this.UpdateAsObservable()
            .Where(_ => !faint)
            .Select(x => RecognizeRange())
            .Where(x => x > 0)
            .Subscribe(x => Recognize(x));

        this.UpdateAsObservable()
            .Where(_ => recognize)
            .Subscribe(_ => Attack());

        this.ObserveEveryValueChanged(_ => destination)
            .Subscribe(_ => Move());

        this.UpdateAsObservable()
            .Where(_ => agent.remainingDistance < agent.stoppingDistance)
            .Subscribe(_ =>
            {
                Rotate(destination);
            });
    }

    void Move()
    {
        agent.speed = monsterData.RunSpeed;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("howl"))
        {
            agent.speed = 0;
        }

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(destination, path))
        {
            animator.SetBool("stop", false);
            agent.SetDestination(destination);
        }
    }

    void Rotate(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        if (Vector3.Distance(destination, transform.position) > 1f)
        {
            animator.SetBool("stop", true);
        }
        else
        {
            recognize = false;
        }
    }

    int RecognizeRange()
    {
        animator.SetBool("recognize", recognize);

        return Physics.OverlapSphereNonAlloc(
            transform.position,
            Mathf.Max(monsterData.SoundRecognitionDist, monsterData.SightRecognitionDist),
            targets,
            soundLayer | playerLayer);
    }

    void Recognize(int size)
    {
        float max = -1;
        Transform target = null;

        for (int i = 0; i < size; i++)
        {
            float targetDist = Vector3.Distance(transform.position, targets[i].transform.position);
            bool isObstacle = Physics.Linecast(transform.position, targets[i].transform.position, obstacleLayer);

            if (targetDist < monsterData.SightRecognitionDist)
            {
                if(monsterData.name.Equals("Gazer") && targets[i].gameObject.CompareTag("sparkler"))
                {
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
                else if(targets[i].gameObject.layer == Math.Log(playerLayer.value, 2))
                {
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
            }
            if(targetDist < monsterData.SoundRecognitionDist
                &&
                targets[i].gameObject.layer == Math.Log(soundLayer.value, 2))
            {
                float distWeight = Mathf.Lerp(1.2f, 0, targetDist / monsterData.SoundRecognitionDist);
                float volumePerDist = targets[i].GetComponent<AudioSource>().volume * distWeight;
                if (volumePerDist > 0.25f && volumePerDist > max)
                {
                    max = volumePerDist;
                    target = targets[i].transform;
                }
            }
        }
        if(target != null)
        {
            recognize = true;
            destination = target.transform.position;
        }
    }

    void Attack()
    {
        bool inRange = Physics.CheckSphere(
            transform.position,
            monsterData.Reach * 2,
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
            monsterData.HitSize,
            transform.rotation,
            playerLayer);

        Debug.Log(hit);
        if (hit)
        {
            
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
        faint = true;
        animator.SetTrigger("bottle");
        StartCoroutine(WakeUp());
    }

    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(awakeTime);
        faint = false;
        animator.SetTrigger("wakeUp");
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(Vector3.zero, monsterData.SightRecognitionDist);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, monsterData.SoundRecognitionDist);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, monsterData.Reach);
        Gizmos.DrawWireCube(Vector3.zero, monsterData.HitSize);

        Vector3 rightDir = AngleToDir(Vector3.forward.y + monsterData.SightRecognitionAngle * 0.5f);
        Vector3 leftDir = AngleToDir(Vector3.forward.y - monsterData.SightRecognitionAngle * 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(Vector3.zero, rightDir * monsterData.SightRecognitionDist);
        Gizmos.DrawRay(Vector3.zero, leftDir * monsterData.SightRecognitionDist);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(Vector3.zero, Vector3.forward * monsterData.SightRecognitionDist);
    }
}
