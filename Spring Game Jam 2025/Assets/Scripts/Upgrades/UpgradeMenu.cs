using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance;

    [Header("Paint Area Upgrades")]
    public string[] PaintAreaLevels;
    public int[] PaintAreaUpgradePrices;

    [Header("Dryer Upgrades")]
    public float[] DryerUpgrades;
    public int[] DryerUpgradePrices;

    [Header("Stencil Upgrades")]
    public float[] StencilUpgrades;
    public int[] StencilUpgradePrices;

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
    }

    public void UpgradePaintArea()
    {
        UpgradeEventManager.OnUpgradePaintArea();
    }

    public void UpgradeDryingSpeed()
    {
        UpgradeEventManager.OnUpgradeDryingSpeed();
    }

    public void UpgradeStencil()
    {
        UpgradeEventManager.OnUpgradeStencil();
    }

    public void OpenUpgradeMenu()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        TimerEventManager.OnTimerStop();
        Time.timeScale = 0f;
    }

    public void CloseUpgradeMenu()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        TimerEventManager.OnTimerStart();
        Time.timeScale = 1f;
    }
}
