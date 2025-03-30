using TMPro;
using UnityEngine;

public class DryerUpgradeOption : MonoBehaviour
{
    [SerializeField] TMP_Text upgradeText;
    [SerializeField] TMP_Text priceText;
    private int currUpgradeIndex = 0;

    void OnEnable()
    {
        UpgradeEventManager.UpgradeDryingSpeed += UpgradeDryingSpeed;
    }

    void OnDisable()
    {
        UpgradeEventManager.UpgradeDryingSpeed -= UpgradeDryingSpeed;
    }

    private void UpgradeDryingSpeed()
    {
        float[] dryerUpgrades = UpgradeMenu.Instance.DryerUpgrades;

        // Update upgrade & price text
        string newUpgradeText;
        string newPriceText;
        if (currUpgradeIndex >= dryerUpgrades.Length - 2)
        {
            newUpgradeText = "Maxed";
            newPriceText = "Maxed";
        }
        else
        {
            currUpgradeIndex++;
            newUpgradeText = dryerUpgrades[currUpgradeIndex] + "s --> " + dryerUpgrades[currUpgradeIndex + 1] + "s";
            newPriceText = UpgradeMenu.Instance.DryerUpgradePrices[currUpgradeIndex] + " G";
        }

        upgradeText.text = newUpgradeText;
        priceText.text = newPriceText;
    }
}
