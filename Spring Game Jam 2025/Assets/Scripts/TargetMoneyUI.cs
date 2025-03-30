using TMPro;
using UnityEngine;

public class TargetMoneyUI : MonoBehaviour
{
    void Update()
    {
        GetComponent<TMP_Text>().text = "Target: $" + GameManager.Instance.TargetMoney;
    }
}
