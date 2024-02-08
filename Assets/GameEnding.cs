using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GameEnding : MonoBehaviour
{
    public GameObject GameEndingObj;
    public bool isOnCollision = false;

    public GameObject GameEndingUI;
    public static bool GameIsPaused = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        this.UpdateAsObservable()
            .Where(_ => isOnCollision == true)
            .Subscribe(_ => GameState());
    }

    public void GameState()
    {
        GameIsPaused = !GameIsPaused;
        Time.timeScale = (Time.timeScale + 1) % 2;
        Cursor.visible = GameIsPaused;
        Cursor.lockState = (CursorLockMode)(((int)Cursor.lockState + 1) % 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnCollision = true;
        }
    }
}
