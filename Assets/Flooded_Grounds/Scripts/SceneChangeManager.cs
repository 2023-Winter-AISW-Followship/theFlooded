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
        //게임 시작 버튼 클릭 시, 스토리텔링 페이지로
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
        //스토리텔링 페이지의 마지막 버튼 클릭 시, 인게임 페이지로
        SceneManager.LoadScene("Scene_InGame");
    }

    public void SceneChange_Settings()
    {
        settingCanvas.Child("setting").SetActive(true);
    }


    //Scene_GameClear & Scene_GameOver
    public void SceneChange_BackToMain()
    {
        //게임 클리어 & 게임 오버 페이지에서 버튼 클릭 시, 메인(시작) 페이지로
        SceneManager.LoadScene("Scene_Start");
    }

}