using UnityEngine;
using TMPro;

public class PaintUpgradeOption : MonoBehaviour
{
    [SerializeField] TMP_Text upgradeText;
    [SerializeField] TMP_Text priceText;

    void OnEnable()
    {
        UpgradeEventManager.UpgradePaintArea += UpgradePaintArea;
    }

    void OnDisable()
    {
        UpgradeEventManager.UpgradePaintArea -= UpgradePaintArea;
    }

    void Start()
    {
        upgradeText.text = UpgradeMenu.Instance.PaintAreaLevels[0] + " --> " + UpgradeMenu.Instance.PaintAreaLevels[1];
        priceText.text = "$" + UpgradeMenu.Instance.PaintAreaUpgradePrices[0];
    }

    private void UpgradePaintArea()
    {
        string[] paintUpgrades = UpgradeMenu.Instance.PaintAreaLevels;
        int currUpgradeIndex = UpgradeMenu.Instance.PaintUpgradeIndex;

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
            UpgradeMenu.Instance.PaintUpgradeIndex = currUpgradeIndex;
            newUpgradeText = paintUpgrades[currUpgradeIndex] + " --> " + paintUpgrades[currUpgradeIndex + 1];
            newPriceText = "$" + UpgradeMenu.Instance.PaintAreaUpgradePrices[currUpgradeIndex];
        }

        upgradeText.text = newUpgradeText;
        priceText.text = newPriceText;
    }
}
