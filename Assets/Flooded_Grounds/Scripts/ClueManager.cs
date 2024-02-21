using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClueManager : MonoBehaviour
{
    static Text Clue_Num;
    public static float Clue_Percentage = 0; //전역변수
    static Text Clue_Percentage_Text;

    GameObject[] Clue;  //단서 전체
    GameObject[] ClueNormal; //일반 단서
    GameObject[] ClueMain;  //핵심 단서(실험 보고서, 연구 보고서)

    int ClueCount;  //단서 총 개수
    int ClueCountNow;  //발견하지 않은 단서 개수
    int ClueFoundNum;  //발견한 단서 개수


    private void Start()
    {
        Clue_Num = GameObject.Find("Canvas/ClueUI/Clue_Num").GetComponent<Text>();
        Clue_Percentage_Text = GameObject.Find("Canvas/ClueUI/Clue_Percentage").GetComponent<Text>();

        Clue_Num.text = string.Empty;
        Clue_Percentage_Text.text = string.Empty;


        Clue = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.layer == LayerMask.NameToLayer("clue"))  //layer: clue 탐색
            .Select(t => t.gameObject)
            .ToArray();

        ClueNormal = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueNormal")).Select(t => t.gameObject).ToArray();  //tag: ClueNormal 탐색

        ClueMain = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueMain")).Select(t => t.gameObject).ToArray();  //tag: ClueMain 탐색


        ClueCount = Clue.Length;
        ClueCountNow = Clue.Length;
        ClueFoundNum = 0;
    }

    private void Update()
    {
        Clue = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.layer == LayerMask.NameToLayer("clue"))  //layer: clue 탐색
            .Select(t => t.gameObject)
            .ToArray();

        if (Clue.Length != ClueCountNow)
        {
            ClueFoundNum++;
            ClueCountNow--;
        }

        Clue_Num.text = ClueFoundNum + " 개";
        Clue_Percentage = (ClueFoundNum * 100 / ClueCount);
        Clue_Percentage_Text.text = Clue_Percentage + " %";
    }
}

