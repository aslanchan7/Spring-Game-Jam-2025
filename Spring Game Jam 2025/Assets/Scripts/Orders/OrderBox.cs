using UnityEngine;

public class OrderBox : MonoBehaviour
{
    [HideInInspector] public Order order;
    public OrderManager orderManager;

    public void TrySell(GameObject item, ClothingItem clothingItem, bool dry, byte[] pixels)
    {
        // Sells the item if completed
        if (CheckOrderCompletion(clothingItem, dry, pixels) == true)
        {
            // Destroy item and remove order for successful sale
            Destroy(item);
            Destroy(gameObject);
            OrderManager.Instance.orders.Remove(order);
        }
    }

    bool CheckOrderCompletion(ClothingItem clothingItem, bool dry, byte[] pixels)
    {
        // Return true if clothes are correct, dry, and accurate >= 95%
        return clothingItem == order.ClothingItem && dry && order.getAccuracy(pixels) >= 0.95f;
    }

    void Update()
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
    }
}
