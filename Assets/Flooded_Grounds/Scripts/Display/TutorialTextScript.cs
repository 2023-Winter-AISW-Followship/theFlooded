using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class TutorialTextScript : MonoBehaviour
{
    public TMP_Text Text;

    public GameObject[] tutorialTriggersArray;
    public string[] contextArray;

    private void Start()
    {
        Text.text = string.Empty;
    }

    private void Update()
    {
        for (int i = 0; i < tutorialTriggersArray.Length; i++)
        {
            TutorialObj tutorialObjScript = tutorialTriggersArray[i].GetComponent<TutorialObj>();

            if (tutorialObjScript != null && tutorialObjScript.isOnCollision)
            {
                StartCoroutine(Show(contextArray[i]));
                tutorialObjScript.isOnCollision = false;
            }
        }
    }

    IEnumerator Show(string dialog)
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            Text.text += dialog[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
}
