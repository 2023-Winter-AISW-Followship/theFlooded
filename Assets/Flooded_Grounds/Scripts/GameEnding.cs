using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    [SerializeField]
    private GameObject GameEndingUI;
    private Text text;

    private void Start()
    {
        this.OnTriggerEnterAsObservable()
            .Subscribe(_ =>
            {
                Pause.GameIsPaused = !Pause.GameIsPaused;
                Time.timeScale = (Time.timeScale + 1) % 2;
                Cursor.visible = Pause.GameIsPaused;
                Cursor.lockState = (CursorLockMode)(((int)Cursor.lockState + 1) % 2);
                GameEndingUI.SetActive(Pause.GameIsPaused);

                UpdateClueFoundPercentageText();
            });
    }

    private void UpdateClueFoundPercentageText()
    {
        text = GameObject.Find("Canvas/GameEnding/EndingClueFound_Percentage").GetComponent<Text>();
        text.text = "발견한 단서:  " + ClueManager.Clue_Percentage + "%";
    }

    public void IsButtonNoPressed()
    {
        Pause.GameIsPaused = !Pause.GameIsPaused;
        Time.timeScale = (Time.timeScale + 1) % 2;
        Cursor.visible = Pause.GameIsPaused;
        Cursor.lockState = (CursorLockMode)(((int)Cursor.lockState + 1) % 2);
        GameEndingUI.SetActive(Pause.GameIsPaused);
    }
}