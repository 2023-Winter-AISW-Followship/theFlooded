using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClueManager : MonoBehaviour
{
    public static float Clue_Percentage = 0; //전역변수
    static Text Clue_Percentage_Text;

    //GameObject[] Clue;  //단서 전체
    GameObject[] ClueNormal; //일반 단서
    GameObject[] ClueMain;  //핵심 단서(실험 보고서, 연구 보고서)

    //int ClueCount;  //단서 총 개수
    int ClueCountAppliedWeight; //단서 총 개수(핵심 단서 가중치 적용)

    int ClueNormalCountNow;  //발견하지 않은 일반 단서 개수
    int ClueMainCountNow;  //발견하지 않은 핵심 단서 개수

    int ClueFoundNum;  //발견한 단서 개수

    private void Start()
    {
        Clue_Percentage_Text = GameObject.Find("Canvas/ClueUI/Clue_Percentage").GetComponent<Text>();

        Clue_Percentage_Text.text = string.Empty;


        //Clue = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
        //    .Where(t => t.gameObject.layer == LayerMask.NameToLayer("clue"))  //layer: clue 탐색
        //    .Select(t => t.gameObject)
        //    .ToArray();

        ClueNormal = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueNormal")).Select(t => t.gameObject).ToArray();  //tag: ClueNormal 탐색

        ClueMain = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueMain")).Select(t => t.gameObject).ToArray();  //tag: ClueMain 탐색


        //ClueCount = Clue.Length;
        ClueCountAppliedWeight = ClueNormal.Length + ( ClueMain.Length * 3 );

        ClueNormalCountNow = ClueNormal.Length;
        ClueMainCountNow = ClueMain.Length * 3;

        ClueFoundNum = 0;

    }

    private void Update()
    {
        //Clue = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
        //    .Where(t => t.gameObject.layer == LayerMask.NameToLayer("clue"))  //layer: clue 탐색
        //    .Select(t => t.gameObject)
        //    .ToArray();

        ClueNormal = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueNormal")).Select(t => t.gameObject).ToArray();  //tag: ClueNormal 탐색

        ClueMain = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueMain")).Select(t => t.gameObject).ToArray();  //tag: ClueMain 탐색


        if (ClueNormal.Length != ClueNormalCountNow)
        {
            ClueFoundNum++;
            ClueNormalCountNow--;
        }
        else if (ClueMain.Length * 3 != ClueMainCountNow)
        {
            ClueFoundNum += 3;
            ClueMainCountNow -= 3;
        }


        Clue_Percentage = (ClueFoundNum * 100 / ClueCountAppliedWeight);
        Clue_Percentage_Text.text = Clue_Percentage + " %";
    }
}

