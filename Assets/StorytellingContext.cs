using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorytellingContext : MonoBehaviour
{
    public TMP_Text Text;
    public TMP_Text TextShadow;
    string context;

    public string[] contextArray; 

    private void Start()
    {
        context = "게임 시작 스토리텔링 설명 구간\r\n어쩌구 마을 침수가 어쩌구\r\n위험하네 뭐네 호기심\r\n구하러 감";
        StartCoroutine(Show(context));
    }
    IEnumerator Show(string dialog)
    {
        Text.text = string.Empty;
        TextShadow.text = string.Empty;

        for (int i = 0; i < dialog.Length; i++)
        {
            Text.text += dialog[i];
            TextShadow.text += dialog[i];
            yield return new WaitForSeconds(0.1f);

        }
    }
}
