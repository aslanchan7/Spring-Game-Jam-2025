using UnityEngine;
using TMPro;

public class PaintUpgradeOption : MonoBehaviour
{
    [SerializeField] TMP_Text upgradeText;
    [SerializeField] TMP_Text priceText;
    private int currUpgradeIndex = 0;

    void OnEnable()
    {
        UpgradeEventManager.UpgradePaintArea += UpgradePaintArea;
    }

    void OnDisable()
    {
        UpgradeEventManager.UpgradePaintArea -= UpgradePaintArea;
    }

    private void UpgradePaintArea()
    {
        string[] paintUpgrades = UpgradeMenu.Instance.PaintAreaLevels;

        // Update upgrade & price text
        string newUpgradeText;
        string newPriceText;
        if (currUpgradeIndex >= paintUpgrades.Length - 2)
        {
            newUpgradeText = "Maxed";
            newPriceText = "Maxed";
        }
        else
        {
            currUpgradeIndex++;
            newUpgradeText = paintUpgrades[currUpgradeIndex] + " --> " + paintUpgrades[currUpgradeIndex + 1];
            newPriceText = UpgradeMenu.Instance.PaintAreaUpgradePrices[currUpgradeIndex] + " G";
        }

        upgradeText.text = newUpgradeText;
        priceText.text = newPriceText;
    }
}
