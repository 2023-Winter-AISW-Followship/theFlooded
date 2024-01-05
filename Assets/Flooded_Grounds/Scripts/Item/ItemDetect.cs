using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ItemDetect : MonoBehaviour
{
    public float raycastDistance = 5f; //인식할 수 있는 범위
    StringBuilder path = new StringBuilder();

    RaycastHit hit;
    Ray ray;

    void Update()
    {
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red); //씬에서 내가 보고있는 방향을 표시

        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, raycastDistance)) //인식할 수 있는 범위 안에서 물체 확인
        {
            GameObject hitObject = hit.collider.gameObject; //주변 물체의 정보를 가져옵니다.

            if (hitObject.layer == 6)
            {   
                path.Clear();
                path.Append(hitObject.transform.GetChild(0).name);
                path.Append("/Axis/Pickup");
                hitObject.transform.Find(path.ToString()).GetComponent<PickupMessage>().detected();
                if (Input.GetKeyDown(KeySetting.key[KeyAction.INTERACTION]))
                {
                    if(Camera.main.transform.childCount != 0)
                    {
                        return;
                    }
                    GameObject temp = hitObject.transform.GetChild(0).gameObject;
                    temp.transform.parent = Camera.main.transform;
                    temp.transform.localPosition = new Vector3(1, -0.7f, 1);
                    temp.transform.localEulerAngles = new Vector3(0, 0, 0);
                    temp.GetComponent<Rigidbody>().useGravity = false;
                    Destroy(hitObject.gameObject);
                }
            }
        }
    }
}