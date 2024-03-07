using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Radio : MonoBehaviour, ItemState
{
    RaycastHit hit;
    Ray ray;
    public float raycastDistance = 5f;
    GameObject point;

    public bool picked { get; set; }
    public GameObject ItemArm { get; set; }
    public GameObject DefaultArm { get; set; }

    public void ChangeArm()
    {
        ItemArm.SetActive(picked);
        DefaultArm.SetActive(!picked);
    }

    bool isGround = false;

    void Start()
    {
        DefaultArm = Camera.main.transform.Find("Hands/hand_right/HandWithNone").gameObject;
        ItemArm = Camera.main.transform.Find("Hands/hand_right/HandWithRadio").gameObject;

        point = GameObject.Find("EventSystem").transform.GetChild(0).gameObject;

        this.UpdateAsObservable()
            .Where(_ => picked)
            .Subscribe(_ => RadioGuide());

        this.UpdateAsObservable()
            .Where(_ => picked && isGround && Input.GetMouseButtonDown(0))
            .Subscribe(_ => PutDown());

        this.ObserveEveryValueChanged(x => picked)
            .Subscribe(_ => ChangeArm());
    }

    void RadioGuide()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, raycastDistance)) //인식할 수 있는 범위 안에서 물체 확인
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground")
                ||
                hit.collider.gameObject.layer == LayerMask.NameToLayer("building"))
            {
                point.transform.SetPositionAndRotation(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                point.SetActive(true);
                isGround = true;
                return;
            }
        }
        point.SetActive(false);
        isGround = false;
    }

    void PutDown()
    {
        ItemArm.GetComponent<Animator>().SetTrigger("putDown");
       Invoke("Setup", 0.8f);
    }

    public void Setup()
    {
        point.SetActive(false);
        picked = false;
        
        transform.parent = null;
        transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        transform.SetPositionAndRotation(point.transform.position, point.transform.rotation);

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;

        Sound.RadioNoise(transform.position);
    }
}
