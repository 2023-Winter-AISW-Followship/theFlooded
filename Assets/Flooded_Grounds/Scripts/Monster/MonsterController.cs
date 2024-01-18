using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterData monsterData;
    public MonsterData MonsterData { set { monsterData = value; } }

    [SerializeField]
    private LayerMask soundLayer;

    private Animator animator;
    private float awakeTime = 10f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    Collider[] target = new Collider[5];
    float max;
    int index;
    float distWeight;
    AudioSource targetSound;
    float volumePerDist;

    private void Update()
    {
        
        int size = Physics.OverlapSphereNonAlloc(
            transform.position,
            monsterData.SoundRecognitionDist,
            target,
            soundLayer);

        max = -1;
        index = -1;

        for (int i = 0; i < size; i++)
        {
            distWeight = Mathf.Lerp(1.2f, 0, Vector3.Distance(transform.position, target[i].gameObject.transform.position) / monsterData.SoundRecognitionDist);
            targetSound = target[i].GetComponent<AudioSource>();
            //Debug.Log(gameObject.name + " " + Vector3.Distance(transform.position, target[i].gameObject.transform.position) + " " + distWeight);
            volumePerDist = targetSound.volume * distWeight;
            if (volumePerDist > 0.25f && volumePerDist > max)
            {
                max = volumePerDist;
                index = i;
            }    
        }
        if (index != -1)
        {
            transform.LookAt(target[index].transform);
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
