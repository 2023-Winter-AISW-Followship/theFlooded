using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialTextScript : MonoBehaviour
{
    private GameObject TutorialText_interactions;

    public TMP_Text Text;

    public GameObject[] tutorialTriggersArray;
    public string[] contextArray;

    IEnumerator current;

    private void Start()
    {
        TutorialText_interactions = gameObject;
        Text.text = string.Empty;
    }

    private void Update()
    {
        for (int i = 0; i < tutorialTriggersArray.Length; i++)
        {
            TutorialObj tutorialObjScript = tutorialTriggersArray[i].GetComponent<TutorialObj>();

            if (tutorialObjScript != null && tutorialObjScript.isOnCollision)
            {
                if(current != null) StopCoroutine(current);
                current = Show(contextArray[i]);
                StartCoroutine(current);
                Destroy(tutorialObjScript);

                if (i == tutorialTriggersArray.Length - 1)
                {
                    Invoke("ShowEnd", 10);
                }
            }

        }
    }

    IEnumerator Show(string dialog)
    {
        Text.text = string.Empty;

        for (int i = 0; i < dialog.Length; i++)
        {
            Text.text += dialog[i];
            yield return new WaitForSeconds(0.07f);
        }

    }

    private void ShowEnd()
    {
        TutorialText_interactions.SetActive(false);
    }
}
