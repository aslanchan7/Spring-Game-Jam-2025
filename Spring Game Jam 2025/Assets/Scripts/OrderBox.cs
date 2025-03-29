using UnityEngine;

public class OrderBox : MonoBehaviour
{
    [HideInInspector] public Order order;
    public OrderManager orderManager;

    public void TrySell(GameObject item)
    {
        if (CheckOrderCompletion() == true)
        {
            // Sell Item
            Destroy(item);
            Destroy(gameObject);
            OrderManager.Instance.orders.Remove(order);
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
