using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PauseMenu;
    public static bool GameIsPaused = false;

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Escape))
            .Subscribe(_ => GameState());
    }

    public void GameState()
    {

        GameIsPaused = !GameIsPaused;
        Time.timeScale = (Time.timeScale + 1) % 2;
        Cursor.visible = GameIsPaused;
        Cursor.lockState = (CursorLockMode)(((int)Cursor.lockState + 1) % 2);
        PauseMenu.SetActive(GameIsPaused);
    }

    public void Setting()
    {

    }

    public void Exit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit(); // 어플리케이션 종료
    #endif
    }
}
