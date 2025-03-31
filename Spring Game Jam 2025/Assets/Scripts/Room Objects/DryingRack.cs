using UnityEngine;
using UnityEngine.UI;

public class DryingRack : MonoBehaviour
{
    [HideInInspector] public float CompletionTime;
    [SerializeField] Slider progressBar;
    [SerializeField] GameObject clothesPin;
    private int currUpgradeIndex;
    private float startTime;
    private float elapsedTime;
    private int lastFrameChildCount = 0;

    void OnEnable()
    {
        UpgradeEventManager.UpgradeDryingSpeed += OnUpgradeDryingSpeed;
    }

    void OnDisable()
    {
        UpgradeEventManager.UpgradeDryingSpeed -= OnUpgradeDryingSpeed;
    }

    void Start()
    {
        currUpgradeIndex = 0;
        CompletionTime = UpgradeMenu.Instance.DryerUpgrades[currUpgradeIndex];
    }

    void Update()
    {
        if (transform.childCount == 3 && lastFrameChildCount == 2)
        {
            startTime = Time.time;
        }

        if (transform.childCount > 2)
        {
            elapsedTime = Time.time - startTime;

            if (elapsedTime >= CompletionTime)
            {
                // Drying Complete
                elapsedTime = 0f;
                progressBar.gameObject.SetActive(false);

                GetComponentInChildren<Clothing>().dry = true;
            }
        }

        if (transform.childCount == 2)
        {
            progressBar.gameObject.SetActive(false);
            clothesPin.SetActive(false);
        }
        else if (elapsedTime == 0f)
        {
            progressBar.gameObject.SetActive(false);
        }
        else
        {
            progressBar.gameObject.SetActive(true);
            progressBar.value = Mathf.Clamp01(elapsedTime / CompletionTime);
            clothesPin.SetActive(true);
        }

        lastFrameChildCount = transform.childCount;
    }

    private void OnUpgradeDryingSpeed()
    {
        if (currUpgradeIndex == UpgradeMenu.Instance.DryerUpgrades.Length - 1) return;
        CompletionTime = UpgradeMenu.Instance.DryerUpgrades[++currUpgradeIndex];
    }
}
