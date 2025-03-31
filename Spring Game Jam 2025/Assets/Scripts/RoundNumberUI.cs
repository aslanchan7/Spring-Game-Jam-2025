using TMPro;
using UnityEngine;

public class RoundNumberUI : MonoBehaviour
{
    void Update()
    {
        GetComponent<TMP_Text>().text = "Round " + GameManager.Instance.round;
    }
}
