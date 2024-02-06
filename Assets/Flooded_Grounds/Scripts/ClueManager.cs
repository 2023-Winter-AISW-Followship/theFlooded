using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClueManager : MonoBehaviour
{
    static Text Clue_Num;
    static Text Clue_Percentage;

    GameObject[] Clue;  //�ܼ� ��ü
    GameObject[] ClueNormal; //�Ϲ� �ܼ�
    GameObject[] ClueMain;  //�ٽ� �ܼ�(���� ����, ���� ����)

    int ClueFoundNum;


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



        Clue_Num.text = ClueFoundNum + "  /  " + Clue.Length + " ��";
        Clue_Percentage.text = (ClueFoundNum * 100 / Clue.Length) + "  /  100 %";
    }

}

