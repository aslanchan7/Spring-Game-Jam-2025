using UnityEngine;
using TMPro;

public class StencilUpgradeOption : MonoBehaviour
{
    [SerializeField] TMP_Text upgradeText;
    [SerializeField] TMP_Text priceText;

    void OnEnable()
    {
        UpgradeEventManager.UpgradeStencil += UpgradeStencil;
    }

    void OnDisable()
    {
        UpgradeEventManager.UpgradeStencil -= UpgradeStencil;
    }

    void Start()
    {
        upgradeText.text = "+" + UpgradeMenu.Instance.StencilUpgrades[0] + "% Sell Price";
        priceText.text = "$" + UpgradeMenu.Instance.StencilUpgradePrices[0];
    }

    private void UpgradeStencil()
    {
        float[] stencilUpgrades = UpgradeMenu.Instance.StencilUpgrades;
        int currUpgradeIndex = UpgradeMenu.Instance.StencilUpgradeIndex;

        // Update upgrade & price text
        string newUpgradeText;
        string newPriceText;
        if (currUpgradeIndex >= stencilUpgrades.Length - 1)
        {
            newUpgradeText = "Maxed";
            newPriceText = "Maxed";
        }
        else
        {
            currUpgradeIndex++;
            UpgradeMenu.Instance.StencilUpgradeIndex = currUpgradeIndex;
            newUpgradeText = "+" + stencilUpgrades[currUpgradeIndex] + "% Sell Price";
            newPriceText = "$" + UpgradeMenu.Instance.StencilUpgradePrices[currUpgradeIndex];
        }

        upgradeText.text = newUpgradeText;
        priceText.text = newPriceText;
    }
}
