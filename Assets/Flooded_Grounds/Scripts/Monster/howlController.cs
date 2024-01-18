using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class howlController : MonoBehaviour
{
    private Animator animator;
    private float awakeTime = 10f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    float recognitionDist;
    LayerMask recognitionTarget;

    private void Start()
    {
        recognitionDist = 50f;
        recognitionTarget = (1 << 8);
    }

    private void Update()
    {
        bool cast = Physics.SphereCast(
            transform.position,
            recognitionDist,
            transform.up,
            out var hit,
            0);

        if (cast)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, recognitionDist / 2.0f);
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
