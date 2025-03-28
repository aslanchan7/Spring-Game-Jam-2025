using UnityEngine;

public class OrderBox : MonoBehaviour
{
    [HideInInspector] public Order order;

    public void TrySell(GameObject item)
    {
        if (CheckOrderCompletion() == true)
        {
            // Sell Item
            Destroy(item);
            Destroy(gameObject);
        }
    }

    bool CheckOrderCompletion()
    {
        // TODO
        return true;
    }

    void Update()
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
    }
}
