using UnityEngine;
using UnityEngine.Audio;

public class Temp : MonoBehaviour
{
    public AudioMixer audioMixer;
    int sound = 0;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.ambientLight = new Color(0.6f, 0.6f, 0.6f);
        audioMixer.SetFloat("item", sound);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(sound == 0)
            {
                sound = -80;
            }
            else
            {
                sound = 0;
            }
            audioMixer.SetFloat("item", sound);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (RenderSettings.fog)
            {
                RenderSettings.fog = false;
                RenderSettings.ambientLight = Color.white;
            }
            else
            {
                RenderSettings.fog = true;
                RenderSettings.ambientLight = new Color(0.6f, 0.6f, 0.6f);
            }
        }
    }
}
