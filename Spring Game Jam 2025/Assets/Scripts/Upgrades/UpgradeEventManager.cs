using UnityEngine.Events;

public class UpgradeEventManager
{
    public static event UnityAction UpgradePaintArea;
    public static event UnityAction UpgradeDryingSpeed;
    public static event UnityAction UpgradeStencil;

    public static void OnUpgradePaintArea() => UpgradePaintArea?.Invoke();
    public static void OnUpgradeDryingSpeed() => UpgradeDryingSpeed?.Invoke();
    public static void OnUpgradeStencil() => UpgradeStencil?.Invoke();
}
