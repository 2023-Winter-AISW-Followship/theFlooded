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
        context = "���� ���� ���丮�ڸ� ���� ����\r\n��¼�� ���� ħ���� ��¼��\r\n�����ϳ� ���� ȣ���\r\n���Ϸ� ��";
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
