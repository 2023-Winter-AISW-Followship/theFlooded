using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] GameObject brokenBottlePrefab;
    bool playerThrow = false;

    void OnCollisionEnter(Collision collision)
    {
        if (playerThrow)
        {
            if (!collision.collider.CompareTag("Player"))
            {
                GameObject obj = collision.gameObject;
                Explode();
                if (obj.CompareTag("enemy"))
                {
                    Debug.Log(obj.tag);
                    obj.GetComponentInParent<howlController>().bottle();
                }
            }
        }
    }

    void Explode()
    {
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, this.transform.position, Quaternion.identity);
        brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();
        Destroy(gameObject);
    }
}