using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ItemDetect : MonoBehaviour
{
    public float raycastDistance = 7f; //인식할 수 있는 범위
    StringBuilder path = new StringBuilder();
    bool detected = false;

    RaycastHit hit;
    Ray ray;

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => detected
                && Input.GetKeyDown(KeySetting.key[KeyAction.INTERACTION])
                && Camera.main.transform.childCount == 1)
            .Subscribe(_ => Pickup());
    }

    void Update()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, raycastDistance)) //인식할 수 있는 범위 안에서 물체 확인
        {
            if (hit.collider.gameObject.layer == 6)
            {
                path.Clear();
                path.Append(hit.collider.gameObject.tag);
                path.Append("(Clone)/Axis/Pickup");
                hit.collider.gameObject.transform.Find(path.ToString()).GetComponent<PickupMessage>().detected();
                detected = true;
            }
        }
    }

    void Pickup()
    {
        GameObject temp = hit.collider.gameObject.transform.GetChild(0).gameObject;
        temp.transform.parent = Camera.main.transform;
        temp.transform.position = new Vector3(0, -100, 0);
        temp.GetComponent<Rigidbody>().useGravity = false;
        temp.GetComponent<Collider>().enabled = false;
        temp.GetComponent<ItemState>().picked = true;
    }
}