using System.Collections;
using UnityEngine;
using TMPro;

public class GameOverContext : MonoBehaviour
{
    public TMP_Text Text;
    public TMP_Text TextShadow;
    GameObject ButtonMove;

    public string[] contextArray;
    private int currentIndex = 0;

    private void Start()
    {
        ButtonMove = GameObject.Find("Canvas/Background/ButtonMove").gameObject;

        StartCoroutine(Show(contextArray[currentIndex]));
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
}
