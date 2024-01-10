using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ItemState
{
    bool picked { get; set; }

    GameObject ItemArm { get; set; }
    GameObject DefaultArm { get; set; }

    void ChangeArm()
    {
        ItemArm.SetActive(picked);
        DefaultArm.SetActive(!picked);
    }
}