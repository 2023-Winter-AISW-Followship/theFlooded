using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Bottle : MonoBehaviour, ItemState
{
    [SerializeField] GameObject brokenBottlePrefab;
    bool playerThrow = false;

    public bool picked { get; set; }

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => picked
                && Input.GetMouseButtonDown(0))
            .Subscribe(_ => Throw());
        
        this.OnCollisionEnterAsObservable()
            .Where(_ => playerThrow)
            .Subscribe(_ => Explode());
    }

    void Throw()
    {
        GetComponent<Rigidbody>().useGravity = true;
        Vector3 speed = new Vector3(0, 1000f, 8000f);
        GetComponent<Rigidbody>().AddRelativeForce(speed);
        GetComponent<Collider>().enabled = true;
        transform.parent = null;
        playerThrow = true;
        picked = false;
    }

    void Explode()
    {
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, transform.position, Quaternion.identity);
        brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();
        Destroy(gameObject);
    }
}