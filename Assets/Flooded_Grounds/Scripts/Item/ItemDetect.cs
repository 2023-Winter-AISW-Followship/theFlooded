using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float raycastDistance = 5f; //인식할 수 있는 범위

    RaycastHit hit;
    Ray ray;

    void Update()
    {
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red); //씬에서 내가 보고있는 방향을 표시

        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, raycastDistance)) //인식할 수 있는 범위 안에서 물체 확인
        {
            GameObject hitObject = hit.collider.gameObject; //주변 물체의 정보를 가져옵니다.

            if (hitObject != null) //물체가 있을 경우
            {
                Debug.Log(hitObject.name);
            }
        }
    }
}