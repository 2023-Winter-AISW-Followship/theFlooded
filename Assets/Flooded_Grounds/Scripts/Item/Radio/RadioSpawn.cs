using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSpawn : MonoBehaviour
{
    [SerializeField] GameObject radioPrefab;

    void Start()
    {
        GameObject temp = Instantiate(radioPrefab, this.transform.position, Quaternion.identity);
        temp.transform.parent = this.transform;
    }
}
