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
        //���� ���� ��ư Ŭ�� ��, ���丮�ڸ� ��������
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
        //���丮�ڸ� �������� ������ ��ư Ŭ�� ��, �ΰ��� ��������
        SceneManager.LoadScene("Scene_InGame");
    }

    public void SceneChange_Settings()
    {
        settingCanvas.Child("setting").SetActive(true);
    }


    //Scene_InGame
    public void SceneChange_GameClear()
    {
        //�߰��� �ܼ� % ������ ���� ���� �б� ������. ����� ���� ���� �� �� ���ǹ��� ���� ��ȯ��.
        //������ �����ٴ� ��ư Ŭ�� ��, ���� Ŭ���� ��������
        Pause.GameIsPaused = !Pause.GameIsPaused;
        Time.timeScale = (Time.timeScale + 1) % 2;

        if (ClueManager.Clue_Percentage < 30.0f) SceneManager.LoadScene("Scene_GameClear_0-30");
        else if (30.0f <= ClueManager.Clue_Percentage && ClueManager.Clue_Percentage < 60.0f) SceneManager.LoadScene("Scene_GameClear_30-60");
        else if (60.0f <= ClueManager.Clue_Percentage) SceneManager.LoadScene("Scene_GameClear_60-100");
        else Debug.Log("���� ���� �� ��ȯ ����");
    }


    //Scene_GameClear & Scene_GameOver
    public void SceneChange_BackToMain()
    {
        //���� Ŭ���� & ���� ���� ���������� ��ư Ŭ�� ��, ����(����) ��������
        SceneManager.LoadScene("Scene_Start");
    }

}