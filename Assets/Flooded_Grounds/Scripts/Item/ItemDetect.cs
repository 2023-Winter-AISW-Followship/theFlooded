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
    bool isClue = false;

    RaycastHit hit;
    Ray ray;

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => detected
                && Input.GetKeyDown(KeySetting.key[KeyAction.INTERACTION])
                && Camera.main.transform.childCount == 2
                && !isClue)
            .Subscribe(_ => Pickup());

        this.UpdateAsObservable()
            .Where(_ => detected
                && Input.GetKeyDown(KeySetting.key[KeyAction.INTERACTION])
                && isClue)
            .Subscribe(_ =>
            {
                Destroy(hit.collider.gameObject);
            });
    }

    void Update()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        detected = false;
        isClue = false;

        if (Physics.Raycast(ray, out hit, raycastDistance, 1 << LayerMask.NameToLayer("item"))) //�ν��� �� �ִ� ���� �ȿ��� ��ü Ȯ��
        {
            path.Clear();
            path.Append(hit.collider.tag);
            path.Append("(Clone)");
            //hit.collider.transform.Find(path.ToString()).GetComponent<Outline>().detected(); //Outline ������ ������
            path.Append("/Axis/Pickup");

            Transform childTransform = hit.collider.transform.Find(path.ToString());

            if (childTransform != null)
            {
                childTransform.GetComponent<PickupMessage>().detected();
                detected = true;
            }
        }

        else if (Physics.Raycast(ray, out hit, raycastDistance, 1 << LayerMask.NameToLayer("clue"))) //�ν��� �� �ִ� ���� �ȿ��� ��ü Ȯ��
        {
            path.Clear();
            //hit.collider.transform.Find(path.ToString()).GetComponent<Outline>().detected(); //Outline ������ ������
            path.Append("Axis/Pickup");

            Transform childTransform = hit.collider.transform.Find(path.ToString());

            if (childTransform != null)
            {
                childTransform.GetComponent<PickupMessage>().detected();
                detected = true;
                isClue = true;
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
        Destroy(hit.collider.gameObject);
    }
}