using System;
using System.Collections;
using UniRx;
using UnityEngine;

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

    private Animator animator;
    private float awakeTime = 10f;

    bool faint = false;

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
        this.UpdateAsObservable()
            .Where(_ => !faint)
            .Select(x => recognizeRange())
            .Where(x => x > 0)
            .Subscribe(x => recognize(x));
    }

    int recognizeRange()
    {
        return Physics.OverlapSphereNonAlloc(
            transform.position,
            Mathf.Max(monsterData.SoundRecognitionDist, monsterData.SightRecognitionDist),
            targets,
            soundLayer | playerLayer);
    }

    void recognize(int size)
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
                Debug.Log(Math.Log(soundLayer.value, 2));
                float distWeight = Mathf.Lerp(1.2f, 0, targetDist / monsterData.SoundRecognitionDist);
                float volumePerDist = targets[i].GetComponent<AudioSource>().volume * distWeight;
                if (volumePerDist > 0.25f && volumePerDist > max)
                {
                    max = volumePerDist;
                    target = targets[i].transform;
                }
            }
        }
        transform.LookAt(target);
        animator.SetBool("recognize", true);
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
        StartCoroutine(wakeUp());
    }

    IEnumerator wakeUp()
    {
        yield return new WaitForSeconds(awakeTime);
        faint = false;
        animator.SetTrigger("wakeUp");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, monsterData.SightRecognitionDist);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, monsterData.SoundRecognitionDist);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monsterData.Reach);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + monsterData.SightRecognitionAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - monsterData.SightRecognitionAngle * 0.5f);


        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, rightDir * monsterData.SightRecognitionDist);
        Gizmos.DrawRay(transform.position, leftDir * monsterData.SightRecognitionDist);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, lookDir * monsterData.SightRecognitionDist);
    }
}
