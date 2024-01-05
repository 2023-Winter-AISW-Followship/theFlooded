using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwBottle : MonoBehaviour
{
    [SerializeField] GameObject bottlePrefab;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            throwbottle();
        }
    }

    void throwbottle()
    {
        GameObject bottle = Instantiate(bottlePrefab, this.transform.position, Quaternion.identity);
        Vector3 speed = new Vector3(50, 10, 0);
        bottle.GetComponent<Rigidbody>().AddForce(speed);
    }
}
