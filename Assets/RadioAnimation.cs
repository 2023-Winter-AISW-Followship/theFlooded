using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine;

public class RadioAnimation : MonoBehaviour, ItemState
{
    public bool picked { get; set; }
    public GameObject ItemArm { get; set; }
    public GameObject DefaultArm { get; set; }

    public void ChangeArm()
    {
        ItemArm.SetActive(picked);
        DefaultArm.SetActive(!picked);
    }

    private void Start()
    {
        DefaultArm = Camera.main.transform.Find("Hands/hand_right/HandWithNone").gameObject;
        ItemArm = Camera.main.transform.Find("Hands/hand_right/HandWithRadio").gameObject;

        this.UpdateAsObservable()
            .Where(_ => picked
                && Input.GetMouseButtonDown(0))
            .Subscribe(_ => PutDown());

        this.ObserveEveryValueChanged(x => picked)
            .Subscribe(_ => ChangeArm());

    }

    public void PutDown()
    {
        ItemArm.GetComponent<Animator>().SetTrigger("putDown");
    }

}
