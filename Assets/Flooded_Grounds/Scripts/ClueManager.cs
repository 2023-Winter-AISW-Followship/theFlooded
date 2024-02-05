using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClueManager : MonoBehaviour
{
    Text Clue_Num;
    Text Clue_Percentage;

    GameObject[] Clue;  //�ܼ� ��ü
    GameObject[] ClueNormal; //�Ϲ� �ܼ�
    GameObject[] ClueMain;  //�ٽ� �ܼ�(���� ����, ���� ����)

    int ClueFoundNum = 0;


    private void Start()
    {
        Clue_Num = GameObject.Find("Canvas/ClueUI/Clue_Num").GetComponent<Text>();
        Clue_Percentage = GameObject.Find("Canvas/ClueUI/Clue_Percentage").GetComponent<Text>();

        Clue_Num.text = string.Empty;
        Clue_Percentage.text = string.Empty;


        GameObject clueParent = GameObject.Find("Clues");
        //�̰Ÿ��� Clue �ؾ���

        ClueNormal = clueParent.GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueNormal")).Select(t => t.gameObject).ToArray();

        ClueMain = clueParent.GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueMain")).Select(t => t.gameObject).ToArray();

        Clue_Num.text = ClueFoundNum + "  /  " + ClueNormal.Length + " ��";
        Clue_Percentage.text = (ClueFoundNum / ClueNormal.Length * 100 ) + "  /  100 %";
    }

}

