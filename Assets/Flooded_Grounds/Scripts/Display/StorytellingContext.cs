using System.Collections;
using UnityEngine;
using TMPro;

public class StorytellingContext : MonoBehaviour
{
    public TMP_Text Text;
    public TMP_Text TextShadow;
    GameObject ButtonMove;
    GameObject ButtonNext;

    public string[] contextArray;
    private int currentIndex = 0;

    private void Start()
    {
        ButtonNext = GameObject.Find("Canvas/Background/ButtonNext").gameObject;
        ButtonMove = GameObject.Find("Canvas/Background/ButtonMove").gameObject;
        ButtonMove.SetActive(false);

        StartCoroutine(Show(contextArray[currentIndex]));
        currentIndex++;
    }

    public void ButtonNext_clicked()
    {
        if (currentIndex < contextArray.Length)
        {
            StartCoroutine(Show(contextArray[currentIndex]));
            currentIndex++;
        }
        else if (currentIndex == contextArray.Length)
        {
            ChangeButton();
        }
    }

    IEnumerator Show(string context)
    {
        Text.text = string.Empty;
        TextShadow.text = string.Empty;

        for (int i = 0; i < context.Length; i++)
        {
            Text.text += context[i];
            TextShadow.text += context[i];
            yield return new WaitForSeconds(0.06f);
        }
    }

    private void ChangeButton()
    {
        ButtonNext.SetActive(false);
        ButtonMove.SetActive(true);
    }
}
