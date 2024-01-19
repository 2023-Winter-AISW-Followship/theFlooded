using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterData monsterData;
    public MonsterData MonsterData { set { monsterData = value; } }

    [SerializeField]
    private LayerMask soundLayer;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask obstacleLayer;

    private Animator animator;
    private float awakeTime = 10f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        
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

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    Vector3 lookDir;
    private void Update()
    {
        lookDir = AngleToDir(transform.eulerAngles.y);

        playerRecognize();
        soundRecognize();
    }

    void playerRecognize()
    {
        Collider[] player = new Collider[1];
        float targetDist;

        bool cast = Physics.OverlapSphereNonAlloc(
            transform.position,
            Mathf.Max(monsterData.SoundRecognitionDist, monsterData.SightRecognitionDist),
            player,
            playerLayer) > 0 ? true : false;

        if (cast)
        {
            targetDist = Vector3.Distance(transform.position, player[0].transform.position);

            if (targetDist < monsterData.SightRecognitionDist && (player[0].gameObject.layer == 9 || monsterData.MonsterName.Equals("gazer")))
            {
                Vector3 targetDir = (player[0].transform.position - transform.position).normalized;
                float targetAngle = Mathf.Acos(Vector3.Dot(AngleToDir(transform.eulerAngles.y), targetDir)) * Mathf.Rad2Deg;
                if (targetAngle <= monsterData.SightRecognitionAngle * 0.5f && !Physics.Raycast(transform.position, targetDir, monsterData.SightRecognitionDist, obstacleLayer))
                {
                    Debug.DrawLine(transform.position, player[0].transform.position, Color.red);
                    transform.LookAt(player[0].transform);
                    animator.SetBool("recognize", true);
                }
            }
        }
    }

    void soundRecognize()
    {
        Collider[] targets = new Collider[5];
        GameObject target;
        float max;
        int index;
        float targetDist;
        float distWeight;
        AudioSource targetSound;
        float volumePerDist;

        int size = Physics.OverlapSphereNonAlloc(
            transform.position,
            Mathf.Max(monsterData.SoundRecognitionDist, monsterData.SightRecognitionDist),
            targets,
            soundLayer);

        max = -1;
        index = -1;

        for (int i = 0; i < size; i++)
        {
            target = targets[i].gameObject;
            targetDist = Vector3.Distance(transform.position, target.transform.position);

            distWeight = Mathf.Lerp(1.2f, 0, targetDist / monsterData.SoundRecognitionDist);
            targetSound = target.GetComponent<AudioSource>();
            volumePerDist = targetSound.volume * distWeight;
            if (volumePerDist > 0.25f && volumePerDist > max)
            {
                max = volumePerDist;
                index = i;
            }
        }
        if (index != -1)
        {
            transform.LookAt(targets[index].transform);
            animator.SetBool("recognize", true);
        }
    }

    public void bottle()
    {
        if (
            animator.GetCurrentAnimatorStateInfo(0).IsName("faint")
            ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("wakeUp")
            )
        {
            return;
        }
        animator.SetTrigger("bottle");
        StartCoroutine(wakeUp());
    }

    IEnumerator wakeUp()
    {
        yield return new WaitForSeconds(awakeTime);
        animator.SetTrigger("wakeUp");
    }
}
