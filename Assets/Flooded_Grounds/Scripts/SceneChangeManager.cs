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


    //Scene_InGame
    public void SceneChange_GameClear()
    {
        //발견한 단서 % 비율에 따라 엔딩 분기 설정됨. 현재는 여러 개의 씬 중 조건문에 따라 전환됨.
        //마을을 떠난다는 버튼 클릭 시, 게임 클리어 페이지로
        Pause.GameIsPaused = !Pause.GameIsPaused;
        Time.timeScale = (Time.timeScale + 1) % 2;

        if (ClueManager.Clue_Percentage < 30.0f) SceneManager.LoadScene("Scene_GameClear_0-30");
        else if (30.0f <= ClueManager.Clue_Percentage && ClueManager.Clue_Percentage < 60.0f) SceneManager.LoadScene("Scene_GameClear_30-60");
        else if (60.0f <= ClueManager.Clue_Percentage) SceneManager.LoadScene("Scene_GameClear_60-100");
        else Debug.Log("게임 엔딩 씬 전환 에러");
    }


    //Scene_GameClear & Scene_GameOver
    public void SceneChange_BackToMain()
    {
        //게임 클리어 & 게임 오버 페이지에서 버튼 클릭 시, 메인(시작) 페이지로
        SceneManager.LoadScene("Scene_Start");
    }

}