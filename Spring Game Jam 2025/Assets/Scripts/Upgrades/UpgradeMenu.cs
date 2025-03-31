using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance;

    [Header("Paint Area Upgrades")]
    public string[] PaintAreaLevels;
    public int[] PaintAreaUpgradePrices;
    [HideInInspector] public int PaintUpgradeIndex = 0;

    [Header("Dryer Upgrades")]
    public float[] DryerUpgrades;
    public int[] DryerUpgradePrices;
    [HideInInspector] public int DryerUpgradeIndex = 0;

    [Header("Stencil Upgrades")]
    public float[] StencilUpgrades;
    public int[] StencilUpgradePrices;
    [HideInInspector] public int StencilUpgradeIndex = 0;

    public AudioSource upgradeSound;

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
        if (GameManager.Instance.Money < PaintAreaUpgradePrices[PaintUpgradeIndex]) return;
        GameManager.Instance.UpdateMoney(-PaintAreaUpgradePrices[PaintUpgradeIndex]);
        UpgradeEventManager.OnUpgradePaintArea();
        upgradeSound.Play();
    }

    public void UpgradeDryingSpeed()
    {
        if (GameManager.Instance.Money < DryerUpgradePrices[DryerUpgradeIndex]) return;
        GameManager.Instance.UpdateMoney(-DryerUpgradePrices[DryerUpgradeIndex]);
        UpgradeEventManager.OnUpgradeDryingSpeed();
        upgradeSound.Play();
    }

    public void UpgradeStencil()
    {
        if (GameManager.Instance.Money < StencilUpgradePrices[StencilUpgradeIndex]) return;
        GameManager.Instance.UpdateMoney(-StencilUpgradePrices[StencilUpgradeIndex]);
        UpgradeEventManager.OnUpgradeStencil();
        upgradeSound.Play();
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
