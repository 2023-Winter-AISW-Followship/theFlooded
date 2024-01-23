using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine;

public class BottleAnimation : MonoBehaviour, ItemState
{
    bool playerThrow = false;
    GameObject bottle;
    public bool picked { get; set; }
    public GameObject ItemArm { get; set; }
    public GameObject DefaultArm { get; set; }

    public void ChangeArm()
    {
        Debug.Log("ChangeArm작동");

        ItemArm.SetActive(picked);
        DefaultArm.SetActive(!picked);
    }

    private void Start()
    {
        DefaultArm = Camera.main.transform.Find("Hands/hand_right/HandWithNone").gameObject;
        ItemArm = transform.gameObject;
        bottle = Camera.main.transform.Find("Hands/hand_right/HandWithBottle/Bottle").gameObject;

        var bottleScript = bottle.GetComponent<Bottle>();

        this.UpdateAsObservable()
            .Where(_ => picked
                && Input.GetMouseButtonDown(0))
            .Subscribe(_ => Throw());

        this.ObserveEveryValueChanged(x => picked)
            .Subscribe(_ => ChangeArm());


        this.OnCollisionEnterAsObservable()
            .Where(_ => playerThrow)
            .Select(x => x.gameObject)
            .Subscribe(x => bottleScript.Explode(x));

    }

    public void Throw()
    {
        ItemArm.GetComponent<Animator>().SetTrigger("throw");

        Invoke("bottleLaunch", .54f);

    }

    //HandWithBottle이 비활성화되는 시점에 병이 날아가야 함
    public void bottleLaunch()
    {
        transform.position = bottle.transform.position;
        transform.rotation = Camera.main.transform.rotation;
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        Vector3 speed = Vector3.forward * 20f + Vector3.up * 5f;
        GetComponent<Rigidbody>().velocity = Camera.main.transform.TransformDirection(speed);
        GetComponent<Collider>().enabled = true;
        playerThrow = true;
        picked = false;
    }

}
