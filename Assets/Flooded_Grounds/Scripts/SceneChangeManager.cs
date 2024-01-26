using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    //Scene_Start
    public void SceneChange_StartGame()
    {
        SceneManager.LoadScene("Scene_StartStorytelling");
    }

    public void SceneChange_Settings()
    {
        SceneManager.LoadScene("Scene_CommandPattern");
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


    //Scene_CommandPattern
    public void SceneChange_SettingsBack()
    {
        SceneManager.LoadScene("Scene_Start");
    }
}
