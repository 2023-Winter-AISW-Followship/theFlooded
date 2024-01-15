using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparklerSpawn : MonoBehaviour
{
    [SerializeField] GameObject sparklerPrefab;

    void Start()
    {
        GameObject temp = Instantiate(sparklerPrefab, this.transform.position, Quaternion.identity);
        temp.transform.parent = this.transform;
    }
}
