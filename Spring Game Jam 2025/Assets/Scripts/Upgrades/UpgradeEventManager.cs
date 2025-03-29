using UnityEngine.Events;

public class UpgradeEventManager
{
    public static event UnityAction UpgradeDryingSpeed;

    public static void OnUpgradeDryingSpeed() => UpgradeDryingSpeed?.Invoke();
}
