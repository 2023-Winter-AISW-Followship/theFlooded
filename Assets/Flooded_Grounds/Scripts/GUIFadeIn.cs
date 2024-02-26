using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GUIFadeIn : MonoBehaviour
{
    public Image Panel;
    public bool IsFadeEnded = false;

    void Start()
    {
        StartCoroutine(FadeInCoroutine());

        this.UpdateAsObservable() //화면 시작 시 FadePanel 비활성화
            .Where(_ => IsFadeEnded == true)
            .Subscribe(_ =>
            {
                Panel.gameObject.SetActive(false);
                IsFadeEnded = false;
            });
    }

    IEnumerator FadeInCoroutine()
    {
        float fadeCount = 1f;
        while (fadeCount >= 0f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            Panel.color = new Color(0,0,0,fadeCount);
        }
        IsFadeEnded = true;
    }
}
