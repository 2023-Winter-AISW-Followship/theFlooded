using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ItemDetect : MonoBehaviour
{
    public float raycastDistance = 7f; //�ν��� �� �ִ� ����
    StringBuilder path = new StringBuilder();
    bool detected = false;

    RaycastHit hit;
    Ray ray;

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => detected
                && Input.GetKeyDown(KeySetting.key[KeyAction.INTERACTION])
                && Camera.main.transform.childCount == 2)
            .Subscribe(_ => Pickup());
    }

    void Update()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        detected = false;
        if (Physics.Raycast(ray, out hit, raycastDistance)) //�ν��� �� �ִ� ���� �ȿ��� ��ü Ȯ��
        {
            if (hit.collider.gameObject.layer == 6) //layer 6: 'item'
            {
                path.Clear();
                path.Append(hit.collider.tag);
                path.Append("(Clone)");
                //hit.collider.transform.Find(path.ToString()).GetComponent<Outline>().detected(); //Outline ������ ������
                path.Append("/Axis/Pickup");

                Transform childTransform = hit.collider.transform.Find(path.ToString());

                if (childTransform != null)
                {
                    hit.collider.transform.Find(path.ToString()).GetComponent<PickupMessage>().detected();
                    detected = true;
                }
                
            }
        }
    }

    void Pickup()
    {
            GameObject temp = hit.collider.transform.GetChild(0).gameObject;
            temp.transform.parent = Camera.main.transform;
            temp.transform.position = new Vector3(0, -100, 0);
            temp.GetComponent<Collider>().enabled = false;
            temp.GetComponent<ItemState>().picked = true;
            Destroy(hit.collider);
    }
}