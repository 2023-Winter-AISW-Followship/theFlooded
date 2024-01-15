using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    GameObject temp;

    public void ChangeArm()
    {
        ItemArm.SetActive(picked);
        DefaultArm.SetActive(!picked);
    }


    private void Start()
    {
        DefaultArm = Camera.main.transform.Find("Hands/hand_right/HandWithNone").gameObject;
        ItemArm = Camera.main.transform.Find("Hands/hand_right/HandWithBottle").gameObject;
        temp = Camera.main.gameObject.Descendants().Where(x => x.name.Equals("Bottle")).FirstOrDefault().gameObject;

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
        transform.position = temp.transform.position;
        transform.rotation = Camera.main.transform.rotation;
        GetComponent<Rigidbody>().useGravity = true;
        Vector3 speed = Vector3.forward * 15000f + Vector3.up * 10000f;
        GetComponent<Rigidbody>().AddForce(Camera.main.transform.TransformDirection(speed));
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