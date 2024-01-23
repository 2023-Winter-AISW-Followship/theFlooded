using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] GameObject brokenBottlePrefab;

    private void Start()
    {

    }

    public void Explode(GameObject collision)
    {
        Sound.BottleExplosion(transform.position);
        
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, transform.position, Quaternion.identity);
        brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();

        if (collision.CompareTag("enemy"))
        {
            Debug.Log(collision.tag);
            collision.GetComponentInParent<MonsterController>().bottle();
        }
        Debug.Log("Explode¿€µø");

        Destroy(gameObject);
    }
}