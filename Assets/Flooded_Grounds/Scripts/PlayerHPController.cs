using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHPController : MonoBehaviour
{
    [SerializeField] private static Slider hpBarSlider;
    private static float curHP;
    private static float maxHP;
    private static float minHP;

    int HpRecoverAmount = 5;

    private void Start()
    {
        hpBarSlider = GameObject.Find("Canvas/PlayerHP").GetComponent<Slider>();

        curHP = hpBarSlider.value;
        maxHP = hpBarSlider.maxValue;
        minHP = hpBarSlider.minValue;

        StartCoroutine(HpRecover());
    }

    public void SetHP(float amount)
    {
        maxHP = amount;
        curHP = maxHP;
        UpdateHpBar();
    }
 
    public static void TakeDamage(float damage)
    {
        if (maxHP == 0 || curHP <= 0) return;

        curHP -= damage;

        UpdateHpBar();

        if (curHP <= 0)
        {
            SceneManager.LoadScene("Scene_GameOver");
        }
    }

    private static void UpdateHpBar()
    {
        if (hpBarSlider != null)
        {
            hpBarSlider.value = curHP;
        }
    }

    private IEnumerator HpRecover()
    {
        if (curHP < maxHP)
        {
            curHP += HpRecoverAmount;
            UpdateHpBar();
        }
        yield return new WaitForSeconds(3);
        StartCoroutine(HpRecover());
    }
}
