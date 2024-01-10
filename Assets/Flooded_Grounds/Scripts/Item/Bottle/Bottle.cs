using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine;

public class Bottle : MonoBehaviour, ItemState
{
    [SerializeField] GameObject brokenBottlePrefab;
    bool playerThrow = false;

    public bool picked { get; set; }
    public GameObject ItemArm {  get; set; }
    public GameObject DefaultArm { get; set; }

    public void ChangeArm()
    {
        ItemArm.SetActive(picked);
        DefaultArm.SetActive(!picked);
    }


    private void Start()
    {
        DefaultArm = Camera.main.transform.Find("HandsNormal/hand_right/HandWithNone").gameObject;
        ItemArm = Camera.main.transform.Find("HandsNormal/hand_right/HandWithBottle").gameObject;

        this.UpdateAsObservable()
            .Where(_ => picked
                && Input.GetMouseButtonDown(0))
            .Subscribe(_ => Throw());
        
        this.OnCollisionEnterAsObservable()
            .Where(_ => playerThrow)
            .Subscribe(_ => Explode());

        this.ObserveEveryValueChanged(x => picked)
            .Subscribe(_ => ChangeArm());
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