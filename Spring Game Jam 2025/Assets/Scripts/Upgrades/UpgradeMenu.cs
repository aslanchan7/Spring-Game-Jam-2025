using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance;

    [Header("Dryer Upgrades")]
    public float[] DryerUpgrades;
    public int[] DryerUpgradePrices;

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

    public void UpgradeDryingSpeed()
    {
        UpgradeEventManager.OnUpgradeDryingSpeed();
    }
}
