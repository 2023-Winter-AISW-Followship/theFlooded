using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClueManager : MonoBehaviour
{
    Text Clue_Num;
    Text Clue_Percentage;

    GameObject[] Clue;  //단서 전체
    GameObject[] ClueNormal; //일반 단서
    GameObject[] ClueMain;  //핵심 단서(실험 보고서, 연구 보고서)

    int ClueFoundNum = 0;


    private void Start()
    {
        Clue_Num = GameObject.Find("Canvas/ClueUI/Clue_Num").GetComponent<Text>();
        Clue_Percentage = GameObject.Find("Canvas/ClueUI/Clue_Percentage").GetComponent<Text>();

        Clue_Num.text = string.Empty;
        Clue_Percentage.text = string.Empty;


        GameObject clueParent = GameObject.Find("Clues");
        //이거말구 Clue 해야함

        ClueNormal = clueParent.GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueNormal")).Select(t => t.gameObject).ToArray();

        ClueMain = clueParent.GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueMain")).Select(t => t.gameObject).ToArray();

        Clue_Num.text = ClueFoundNum + "  /  " + ClueNormal.Length + " 개";
        Clue_Percentage.text = (ClueFoundNum / ClueNormal.Length * 100 ) + "  /  100 %";
    }

}

