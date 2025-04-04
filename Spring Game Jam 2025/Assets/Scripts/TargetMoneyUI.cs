using System.Collections;
using TMPro;
using UnityEngine;

public class TargetMoneyUI : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.TargetMoneyUpdated += TargetMoneyUpdated;
    }

    void OnDisable()
    {
        GameManager.TargetMoneyUpdated -= TargetMoneyUpdated;
    }
    public void TargetMoneyUpdated(int value)
    {
        StartCoroutine(UpdateMoneyAnimation(value));
    }

    private IEnumerator UpdateMoneyAnimation(int value)
    {
        float currentTarget = GameManager.Instance.TargetMoney;
        float finalTarget = currentTarget - value;
        if(finalTarget <= 0) {
            finalTarget = GameManager.Instance.initialMoney * (int) (System.Math.Pow(2, GameManager.Instance.round));
        }
        float increment = (float)value / GameManager.Instance.AnimIncrements;

        while (currentTarget >= finalTarget)
        {
            currentTarget -= increment;
            GetComponent<TMP_Text>().text = "$" + (int)currentTarget + " left";
            yield return null;
        }

        GetComponent<TMP_Text>().text = "$" + (int)finalTarget + " left";
    }
}
