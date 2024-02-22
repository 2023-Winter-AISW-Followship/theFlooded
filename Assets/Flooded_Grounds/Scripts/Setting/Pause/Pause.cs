using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PauseMenu;
    public static bool GameIsPaused = false;
    private GameObject settingCanvas;
    private GameObject endCanvas;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        settingCanvas = GameObject.Find("KeySetting");
        endCanvas = gameObject.Descendants().Where(x => x.name.Equals("GameEnding")).FirstOrDefault().gameObject;

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
        endCanvas.SetActive(false);
    }

    public void Setting()
    {
        settingCanvas.Child("setting").SetActive(true);
    }
}
