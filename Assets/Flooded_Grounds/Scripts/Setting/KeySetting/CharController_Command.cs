using System.Collections.Generic;
using Unity.Linq;
using UnityEngine;

public enum KeyAction
{
    FORWARD,
    BACK,
    LEFT,
    RIGHT,
    JUMP,
    RUN,
    SIT,
    INTERACTION,
    COUNT
}

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> key = new Dictionary<KeyAction, KeyCode>();
}

public class CharController_Command : MonoBehaviour
{
    KeyCode[] defaultKey = new KeyCode[]
    {
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D,
        KeyCode.Space,
        KeyCode.LeftShift,
        KeyCode.LeftControl,
        KeyCode.E,
    };

    private static CharController_Command instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            for (int i = 0; i < defaultKey.Length; i++)
            {
                KeySetting.key.Add((KeyAction)i, defaultKey[i]);
            }

            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);
    }

    int keyNum = -1;
    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            if(keyNum != -1)
            {
                for (int i = 0; i < (int)KeyAction.COUNT; i++)
                {
                    if(KeySetting.key[(KeyAction)i] == keyEvent.keyCode)
                    {
                        KeySetting.key[(KeyAction)i] = KeyCode.None;
                    }
                }
                KeySetting.key[(KeyAction)keyNum] = keyEvent.keyCode;
                keyNum = -1;
            }
        }
    }

    public void ChangeKey(int num)
    {
        keyNum = num;
    }

    public void CompleteSetting()
    {
        gameObject.Child("setting").SetActive(false);
    }
}
