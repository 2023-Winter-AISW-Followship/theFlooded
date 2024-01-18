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
            .Select(x => x.gameObject)
            .Subscribe(x => Explode(x));

        this.ObserveEveryValueChanged(x => picked)
            .Subscribe(_ => ChangeArm());
    }

    void Throw()
    {
        ItemArm.GetComponent<Animator>().SetTrigger("throw");

        Invoke("bottleLaunch", .84f);
    }

    void bottleLaunch()
    {
        transform.position = temp.transform.position;
        transform.rotation = Camera.main.transform.rotation;
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        Vector3 speed = Vector3.forward * 20f + Vector3.up * 5f;
        GetComponent<Rigidbody>().velocity = Camera.main.transform.TransformDirection(speed);
        GetComponent<Collider>().enabled = true;
        playerThrow = true;
        picked = false;
    }

    void Explode(GameObject collision)
    {
        Sound.BottleExplosion(transform.position);
        
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, transform.position, Quaternion.identity);
        brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();

        if (collision.CompareTag("enemy"))
        {
            Debug.Log(collision.tag);
            collision.GetComponentInParent<MonsterController>().bottle();
        }

        Destroy(gameObject);
    }
}