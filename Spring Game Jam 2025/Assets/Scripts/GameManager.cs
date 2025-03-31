using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Money")]
    public int Money { get; private set; }
    public int AnimIncrements = 100; // The greater the number, the slower the anim
    public TMP_Text MoneyText;

    public int TargetMoney;
    public Timer RoundTimer;
    public GameObject GameOverScreen;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Time.timeScale = 1f;
    }

    void Start()
    {
        MoneyText.text = "$" + Money;
    }

    public void UpdateMoney(int value)
    {
        StartCoroutine(UpdateMoneyAnimation(value));
        Money += value;
    }

    private IEnumerator UpdateMoneyAnimation(int value)
    {
        float currentMoney = Money;
        float finalMoney = Money + value;
        float increment = (float)value / AnimIncrements;
        if (value > 0)
        {
            while (currentMoney <= finalMoney)
            {
                currentMoney += increment;
                MoneyText.text = "$" + (int)currentMoney;
                yield return null;
            }
        }
        else
        {
            while (currentMoney >= finalMoney)
            {
                currentMoney += increment;
                MoneyText.text = "$" + (int)currentMoney;
                yield return null;
            }
        }
        MoneyText.text = "$" + (int)finalMoney;
    }

    private void Update()
    {
        if (Money >= TargetMoney)
        {
            SetNewTargetMoney();
        }

        if (RoundTimer.TimeToDisplay <= 0f)
        {
            GameOverScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void SetNewTargetMoney()
    {
        TargetMoney *= 2;
        RoundTimer.TimeToDisplay = 120f;
    }
}
