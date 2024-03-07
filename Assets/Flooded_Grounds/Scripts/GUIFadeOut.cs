using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class GUIFadeOut : MonoBehaviour
{
    private GameObject Panel;
    private Image panelImage;
    private bool isFading;

    void Start()
    {
        Panel = GameObject.Find("Canvas/FadeOutPanel").gameObject;
        panelImage = Panel.GetComponent<Image>();
        isFading = false;
        //Panel.SetActive(false);

        this.UpdateAsObservable() // 게임오버 시 FadePanel 활성화
            .Where(_ => PlayerHPController.isGameOver == true && isFading == false)
            .Subscribe(_ =>
            {
                Pause.GameIsPaused = false;
                Time.timeScale = 1;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                isFading = true;
                Panel.SetActive(true);
                StartCoroutine(FadeOutCoroutine());
            });
    }

    IEnumerator FadeOutCoroutine()
    {
        float fadeCount = 0f;
        while (fadeCount <= 1.0f)
        {
            fadeCount += 0.1f;
            yield return new WaitForSeconds(0.1f);
            panelImage.color = new Color(0, 0, 0, fadeCount);
        }

        SceneManager.LoadScene("Scene_GameOver");
    }
}
