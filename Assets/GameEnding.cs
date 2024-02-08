using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    [SerializeField]
    private GameObject GameEndingUI;

    private void Start()
    {
        this.OnTriggerEnterAsObservable()
            .Subscribe(_ =>
            {
                Pause.GameIsPaused = !Pause.GameIsPaused;
                Time.timeScale = (Time.timeScale + 1) % 2;
                Cursor.visible = Pause.GameIsPaused;
                Cursor.lockState = (CursorLockMode)(((int)Cursor.lockState + 1) % 2);
                GameEndingUI.SetActive(true);
            });
    }
}
