using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTextScript : MonoBehaviour
{
    public TMP_Text Text;

    public GameObject[] tutorialTriggersArray;
    private BoxCollider[] objColliderArray; // ∞¢ obj¿« boxcollider
    public string[] contextArray;
    public string[] paramArray;

    public int textNum;
    private int touchedColliderIndex = -1; // Index of the collider touched by the player

    private void Start()
    {
        objColliderArray = new BoxCollider[tutorialTriggersArray.Length];

        for (int i = 0; i < tutorialTriggersArray.Length; i++)
        {
            objColliderArray[i] = tutorialTriggersArray[i].GetComponent<BoxCollider>();
        }
    }

    private void Update()
    {
        for (int i = 0; i < tutorialTriggersArray.Length; i++)
        {
            if (objColliderArray[i].bounds.Intersects(GetComponent<Collider>().bounds)) // Assuming your player has a Collider component
            {
                touchedColliderIndex = i;
                StartTutorial(contextArray);
                break;
            }
        }
    }

    private void StartTutorial(string[] messages)
    {
        paramArray = messages;
        StartCoroutine(Show(paramArray[touchedColliderIndex]));
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
        touchedColliderIndex = -1; // Reset the touched collider index
    }

    IEnumerator Show(string dialog)
    {
        Text.text = string.Empty;
        yield return new WaitForSeconds(5f); // Adjust this if you want a delay between messages
        Text.text = dialog;
        NextMessage();
    }
}
