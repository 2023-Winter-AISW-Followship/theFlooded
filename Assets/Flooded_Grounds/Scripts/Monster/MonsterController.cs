using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterData monsterData;
    public MonsterData MonsterData { set { monsterData = value; } }

    private Animator animator;
    private float awakeTime = 10f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        bool cast = Physics.SphereCast(
            transform.position,
            monsterData.RecognitionDist,
            transform.up,
            out var hit,
            0);

        if (cast)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    void recognize()
    {
        animator.SetBool("recognize", true);
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
