using TMPro;
using UnityEngine;

public class DryerUpgradeOption : MonoBehaviour
{
    [SerializeField] TMP_Text upgradeText;
    [SerializeField] TMP_Text priceText;

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
        int currUpgradeIndex = UpgradeMenu.Instance.DryerUpgradeIndex;

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
            UpgradeMenu.Instance.DryerUpgradeIndex = currUpgradeIndex;
            newUpgradeText = dryerUpgrades[currUpgradeIndex] + "s --> " + dryerUpgrades[currUpgradeIndex + 1] + "s";
            newPriceText = UpgradeMenu.Instance.DryerUpgradePrices[currUpgradeIndex] + " G";
        }

        upgradeText.text = newUpgradeText;
        priceText.text = newPriceText;
    }
}
