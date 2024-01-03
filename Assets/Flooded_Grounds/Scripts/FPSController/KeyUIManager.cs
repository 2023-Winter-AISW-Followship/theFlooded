using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyUIManager : MonoBehaviour
{
    public TextMeshProUGUI[] keyState;
    void Update()
    {
        for(int i = 0; i < keyState.Length; i++)
        {
            keyState[i].text = KeySetting.key[(KeyAction)i].ToString();
        }
    }
}
