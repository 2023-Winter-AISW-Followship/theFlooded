using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleSpawn : MonoBehaviour
{
    [SerializeField] GameObject bottlePrefab;

    void Start()
    {
        GameObject temp = Instantiate(bottlePrefab, this.transform.position, Quaternion.identity);
        temp.transform.parent = this.transform;
    }
}
