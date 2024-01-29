using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Linq;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    private GameObject settingCanvas;
    private void Start()
    {
        settingCanvas = GameObject.Find("KeySetting");
    }

    //Scene_Start
    public void SceneChange_StartGame()
    {
        SceneManager.LoadScene("Scene_StartStorytelling");
    }

    public void SceneChange_ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    //Scene_StartStorytelling
    public void SceneChange_Ingame()
    {
        SceneManager.LoadScene("Scene_InGame");
    }

    public void SceneChange_Settings()
    {
        settingCanvas.Child("setting").SetActive(true);
    }
}