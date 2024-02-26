using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHPController : MonoBehaviour
{
    [SerializeField] 
    private static Slider hpBarSlider;
    private static float curHP;
    private static float maxHP;
    private static float minHP;

    [SerializeField]
    public static bool isGameOver;

    int HpRecoverAmount = 5;

    private void Start()
    {
        hpBarSlider = GameObject.Find("Canvas/PlayerHP").GetComponent<Slider>();

        curHP = hpBarSlider.value;
        maxHP = hpBarSlider.maxValue;
        minHP = hpBarSlider.minValue;
        isGameOver = false;

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
        curHP -= damage;
        if (curHP <= 0) isGameOver = true;

        UpdateHpBar();
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
