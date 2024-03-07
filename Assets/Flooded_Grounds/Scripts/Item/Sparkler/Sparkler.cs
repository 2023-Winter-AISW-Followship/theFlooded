using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Sparkler : MonoBehaviour, ItemState
{
    RaycastHit hit;
    Ray ray;
    public float raycastDistance = 5f;
    GameObject point;

    public bool picked { get; set; }
    public GameObject ItemArm { get; set; }
    public GameObject DefaultArm { get; set; }

    [SerializeField]
    private ParticleSystem[] particles;
    [SerializeField]
    private GameObject sparklePoint;

    public void ChangeArm()
    {
        ItemArm.SetActive(picked);
        DefaultArm.SetActive(!picked);
    }

    bool isGround = false;

    void Start()
    {
        DefaultArm = Camera.main.transform.Find("Hands/hand_right/HandWithNone").gameObject;
        ItemArm = Camera.main.transform.Find("Hands/hand_right/HandWithSparkler").gameObject;

        point = GameObject.Find("EventSystem").transform.GetChild(1).gameObject;

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Pause();
        }

        this.UpdateAsObservable()
            .Where(_ => picked)
            .Subscribe(_ => SparklerGuide());

        this.UpdateAsObservable()
            .Where(_ => picked && isGround && Input.GetMouseButtonDown(0))
            .Subscribe(_ => PutDown());

        this.ObserveEveryValueChanged(x => picked)
            .Subscribe(_ => ChangeArm());
    }

    void SparklerGuide()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, raycastDistance)) //인식할 수 있는 범위 안에서 물체 확인
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground")
                ||
                hit.collider.gameObject.layer == LayerMask.NameToLayer("building"))
            {
                point.transform.position = hit.point + Vector3.up * 0.2f;
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

        Invoke("Setup", .62f);
    }

    void Setup()
    {
        point.SetActive(false);
        picked = false;

        transform.parent = null;
        transform.localScale = new Vector3(0.02f, 0.2f, 0.02f);
        transform.SetPositionAndRotation(hit.point + Vector3.up * 0.2f, point.transform.rotation);

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
        GetComponent<Collider>().enabled = true;

        StartCoroutine(duration(Sound.SparklerSparkle(transform.position)));
    }

    IEnumerator duration(AudioClip clip)
    {
        float time = clip.length;
        float elapsedTime = 0f;

        while (time > elapsedTime)
        {
            sparklePoint.transform.localPosition = Vector3.Lerp(
                new Vector3(0, 1, 0),
                new Vector3(0, -0.5f, 0),
                elapsedTime / time);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(sparklePoint);

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

        yield break;
    }
}
