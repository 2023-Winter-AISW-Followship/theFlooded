using UnityEngine;

public class TutorialObj : MonoBehaviour
{
    public GameObject obj;
    public bool isOnCollision = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnCollision = true;
        }
    }
}
