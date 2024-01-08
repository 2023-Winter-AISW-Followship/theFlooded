using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] GameObject brokenBottlePrefab;
    bool playerThrow = false;

    private void Update()
    {
        if (this.transform.parent == Camera.main.transform && Input.GetMouseButtonDown(0))
        {
            playerThrow = true;
            this.GetComponent<Rigidbody>().useGravity = true;
            Vector3 speed = new Vector3(0, 1000f, 8000f);
            this.GetComponent<Rigidbody>().AddRelativeForce(speed);
            this.GetComponent<Collider>().enabled = true;
            this.transform.parent = null;
        }
    }

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