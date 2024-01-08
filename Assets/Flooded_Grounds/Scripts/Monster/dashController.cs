using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashController : MonoBehaviour
{
    private Animator animator;
    private float awakeTime = 10f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {

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
