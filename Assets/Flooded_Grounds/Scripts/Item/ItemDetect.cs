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
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, raycastDistance)) //인식할 수 있는 범위 안에서 물체 확인
        {

            if (hit.collider.gameObject.layer == 6)
            {   
                path.Clear();
                path.Append(hit.collider.gameObject.transform.GetChild(0).name);
                path.Append("/Axis/Pickup");
                hit.collider.gameObject.transform.Find(path.ToString()).GetComponent<PickupMessage>().detected();
                if (Input.GetKeyDown(KeySetting.key[KeyAction.INTERACTION]))
                {
                    if(Camera.main.transform.childCount != 0)
                    {
                        return;
                    }
                    GameObject temp = hit.collider.gameObject.transform.GetChild(0).gameObject;
                    temp.transform.parent = Camera.main.transform;
                    temp.transform.localPosition = new Vector3(1, -0.7f, 1);
                    temp.transform.localEulerAngles = new Vector3(0, 0, 0);
                    temp.GetComponent<Rigidbody>().useGravity = false;
                    Destroy(hit.collider.gameObject.gameObject);
                }
            }
        }
    }
}