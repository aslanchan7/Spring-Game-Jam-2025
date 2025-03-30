using UnityEngine;
using TMPro;

public class StencilUpgradeOption : MonoBehaviour
{
    [SerializeField] TMP_Text upgradeText;
    [SerializeField] TMP_Text priceText;
    private int currUpgradeIndex = 0;

    void OnEnable()
    {
        UpgradeEventManager.UpgradeStencil += UpgradeStencil;
    }

    void OnDisable()
    {
        UpgradeEventManager.UpgradeStencil -= UpgradeStencil;
    }

    private void UpgradeStencil()
    {
        float[] stencilUpgrades = UpgradeMenu.Instance.StencilUpgrades;

        // Update upgrade & price text
        string newUpgradeText;
        string newPriceText;
        if (currUpgradeIndex >= stencilUpgrades.Length - 2)
        {
            newUpgradeText = "Maxed";
            newPriceText = "Maxed";
        }
        else
        {
            currUpgradeIndex++;
            newUpgradeText = "+" + stencilUpgrades[currUpgradeIndex + 1] + "% Sell Price";
            newPriceText = UpgradeMenu.Instance.StencilUpgradePrices[currUpgradeIndex] + " G";
        }

        upgradeText.text = newUpgradeText;
        priceText.text = newPriceText;
    }
}
