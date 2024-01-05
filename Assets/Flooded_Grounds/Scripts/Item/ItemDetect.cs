using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ItemDetect : MonoBehaviour
{
    public float raycastDistance = 5f; //�ν��� �� �ִ� ����
    StringBuilder path = new StringBuilder();

    RaycastHit hit;
    Ray ray;

    void Update()
    {
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red); //������ ���� �����ִ� ������ ǥ��

        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, raycastDistance)) //�ν��� �� �ִ� ���� �ȿ��� ��ü Ȯ��
        {
            GameObject hitObject = hit.collider.gameObject; //�ֺ� ��ü�� ������ �����ɴϴ�.

            if (hitObject.layer == 6)
            {   
                path.Clear();
                path.Append(hitObject.transform.GetChild(0).name);
                path.Append("/Axis/Pickup");
                hitObject.transform.Find(path.ToString()).GetComponent<PickupMessage>().detected();
                if (Input.GetKeyDown(KeySetting.key[KeyAction.INTERACTION]))
                {
                    Destroy(hitObject.gameObject);
                }
            }
        }
    }
}