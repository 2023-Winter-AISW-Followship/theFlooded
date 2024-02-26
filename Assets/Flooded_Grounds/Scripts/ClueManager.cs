using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClueManager : MonoBehaviour
{
    public static float Clue_Percentage = 0; //��������
    static Text Clue_Percentage_Text;

    //GameObject[] Clue;  //�ܼ� ��ü
    GameObject[] ClueNormal; //�Ϲ� �ܼ�
    GameObject[] ClueMain;  //�ٽ� �ܼ�(���� ����, ���� ����)

    //int ClueCount;  //�ܼ� �� ����
    int ClueCountAppliedWeight; //�ܼ� �� ����(�ٽ� �ܼ� ����ġ ����)

    int ClueNormalCountNow;  //�߰����� ���� �Ϲ� �ܼ� ����
    int ClueMainCountNow;  //�߰����� ���� �ٽ� �ܼ� ����

    int ClueFoundNum;  //�߰��� �ܼ� ����

    private void Start()
    {
        Clue_Percentage_Text = GameObject.Find("Canvas/ClueUI/Clue_Percentage").GetComponent<Text>();

        Clue_Percentage_Text.text = string.Empty;


        //Clue = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
        //    .Where(t => t.gameObject.layer == LayerMask.NameToLayer("clue"))  //layer: clue Ž��
        //    .Select(t => t.gameObject)
        //    .ToArray();

        ClueNormal = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueNormal")).Select(t => t.gameObject).ToArray();  //tag: ClueNormal Ž��

        ClueMain = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueMain")).Select(t => t.gameObject).ToArray();  //tag: ClueMain Ž��


        //ClueCount = Clue.Length;
        ClueCountAppliedWeight = ClueNormal.Length + ( ClueMain.Length * 3 );

        ClueNormalCountNow = ClueNormal.Length;
        ClueMainCountNow = ClueMain.Length * 3;

        ClueFoundNum = 0;

    }

    private void Update()
    {
        //Clue = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
        //    .Where(t => t.gameObject.layer == LayerMask.NameToLayer("clue"))  //layer: clue Ž��
        //    .Select(t => t.gameObject)
        //    .ToArray();

        ClueNormal = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueNormal")).Select(t => t.gameObject).ToArray();  //tag: ClueNormal Ž��

        ClueMain = GameObject.Find("Clues").GetComponentsInChildren<Transform>(true)
            .Where(t => t.gameObject.CompareTag("ClueMain")).Select(t => t.gameObject).ToArray();  //tag: ClueMain Ž��


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

