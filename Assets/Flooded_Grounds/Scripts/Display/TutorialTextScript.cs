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
                //StartCoroutine(DestroyTutorial(contextArray[i]));
                Destroy(tutorialObjScript);
            }
        }
    }

    IEnumerator Show(string dialog)
    {
        Text.text = string.Empty;

        for (int i = 0; i < dialog.Length; i++)
        {
            Text.text += dialog[i];
            yield return new WaitForSeconds(0.03f);
        }

    }

    //IEnumerator DestroyTutorial(string context)
    //{
    //    yield return new WaitForSeconds(4);
    //    context = string.Empty;
    //}
}
