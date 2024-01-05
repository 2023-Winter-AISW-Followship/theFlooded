using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float raycastDistance = 5f; //�ν��� �� �ִ� ����

    RaycastHit hit;
    Ray ray;

    void Update()
    {
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red); //������ ���� �����ִ� ������ ǥ��

        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, raycastDistance)) //�ν��� �� �ִ� ���� �ȿ��� ��ü Ȯ��
        {
            GameObject hitObject = hit.collider.gameObject; //�ֺ� ��ü�� ������ �����ɴϴ�.

            if (hitObject != null) //��ü�� ���� ���
            {
                Debug.Log(hitObject.name);
            }
        }
    }
}