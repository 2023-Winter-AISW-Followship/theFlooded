using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClueManager : MonoBehaviour
{
    static Text Clue_Num;
    static Text Clue_Percentage;

    GameObject[] Clue;  //�ܼ� ��ü
    GameObject[] ClueNormal; //�Ϲ� �ܼ�
    GameObject[] ClueMain;  //�ٽ� �ܼ�(���� ����, ���� ����)

    int ClueCount;  //�ܼ� �� ����
    int ClueCountNow;  //�߰����� ���� �ܼ� ����
    int ClueFoundNum;  //�߰��� �ܼ� ����


    private void Start()
    {
        Clue_Num = GameObject.Find("Canvas/ClueUI/Clue_Num").GetComponent<Text>();
        Clue_Percentage = GameObject.Find("Canvas/ClueUI/Clue_Percentage").GetComponent<Text>();

        Clue_Num.text = string.Empty;
        Clue_Percentage.text = string.Empty;


        Clue = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.layer == LayerMask.NameToLayer("clue"))  //layer: clue Ž��
            .Select(t => t.gameObject)
            .ToArray();

        ClueNormal = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueNormal")).Select(t => t.gameObject).ToArray();  //tag: ClueNormal Ž��

        ClueMain = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueMain")).Select(t => t.gameObject).ToArray();  //tag: ClueMain Ž��


        ClueFoundNum = 0;
        ClueCount = Clue.Length;
        ClueCountNow = Clue.Length;
    }

    private void Update()
    {
        Clue = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.layer == LayerMask.NameToLayer("clue"))  //layer: clue Ž��
            .Select(t => t.gameObject)
            .ToArray();

        if (Clue.Length != ClueCountNow)
        {
            ClueFoundNum++;
            ClueCountNow--;
        }

        if (ClueFoundNum == ClueCount) SceneManager.LoadScene("Scene_GameClear");

        Clue_Num.text = ClueFoundNum + " ��";
        Clue_Percentage.text = (ClueFoundNum * 100 / ClueCount) + " %";
    }

}

