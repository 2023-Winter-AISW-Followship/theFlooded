using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TutorialTextScript : MonoBehaviour
{
    public TMP_Text Text;
    string context;

    public string[] contextArray;
    public string[] paramArray;

    public int textNum;

    private void Start()
    {
        StartTutorial(contextArray);
    }
    private void StartTutorial(string[] messages)
    {
        paramArray = messages;
        StartCoroutine(Show(paramArray[textNum]));
    }

    public void NextMessage()
    {
        Text.text = string.Empty;
        textNum++;

        if (textNum == paramArray.Length)
        {
            EndMessage();
            return;
        }

        StartCoroutine(Show(paramArray[textNum]));
    }

    public void EndMessage()
    {
        textNum = 0;
    }

    IEnumerator Show(string dialog)
    {
        Text.text = string.Empty;

        //게임 시작 초반부 튜토리얼 및 게임 팁 로직 짜야함
        Text.text = dialog;
        yield return new WaitForSeconds(5f);
        NextMessage();
    }
}
