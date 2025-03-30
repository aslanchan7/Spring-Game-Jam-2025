using UnityEngine;

public class StencilManager : MonoBehaviour
{
    public int ActiveStencils;

    void OnEnable()
    {
        UpgradeEventManager.UpgradeStencil += UpgradeStencil;
    }

    void UpgradeStencil()
    {
        if (ActiveStencils >= UpgradeMenu.Instance.StencilUpgrades.Length + 1) return;
        ActiveStencils++;

        for (int i = 0; i < ActiveStencils; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void Start()
    {
        ActiveStencils = 1;
    }

    void Update()
    {

    }
}
