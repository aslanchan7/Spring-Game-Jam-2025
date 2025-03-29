using System.Linq;
using TMPro;
using UnityEngine;

public class UpgradeOption : MonoBehaviour
{
    [SerializeField] TMP_Text upgradeText;
    [SerializeField] TMP_Text priceText;
    private int currUpgradeIndex = 0;

    void OnEnable()
    {
        UpgradeEventManager.UpgradeDryingSpeed += UpgradeDryingSpeed;
    }

    private void UpgradeDryingSpeed()
    {
        float[] dryerUpgrades = UpgradeMenu.Instance.DryerUpgrades;

        // Update upgrade & price text
        string newUpgradeText;
        string newPriceText;
        if (currUpgradeIndex >= dryerUpgrades.Length - 2)
        {
            newUpgradeText = "MAXED";
            newPriceText = "MAXED";
        }
        else
        {
            currUpgradeIndex++;
            newUpgradeText = dryerUpgrades[currUpgradeIndex] + " --> " + dryerUpgrades[currUpgradeIndex + 1];
            newPriceText = UpgradeMenu.Instance.DryerUpgradePrices[currUpgradeIndex] + " G";
        }

        upgradeText.text = newUpgradeText;
        priceText.text = newPriceText;
    }
}
