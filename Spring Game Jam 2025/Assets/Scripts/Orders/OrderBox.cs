using UnityEngine;
using UnityEngine.UI;

public class OrderBox : MonoBehaviour
{
    [HideInInspector] public Order order;
    [HideInInspector] public OrderManager orderManager;
    [HideInInspector] public bool isCurrentOrder;
    [SerializeField] Image image;

    void Start()
    {
        Sprite orderSprite = order.generateSprite();
        image.sprite = orderSprite;
    }

    void Update()
    {
        if (OrderManager.Instance.currentOrder == this)
        {
            isCurrentOrder = true;
        }
        else
        {
            isCurrentOrder = false;
        }

        GetComponent<RectTransform>().localScale = Vector3.one; // this sets the localScale to 1 when mouse is not hovering over it
    }

    public void TrySell(GameObject item, ClothingItem clothingItem, bool dry, byte[] pixels)
    {
        if (isCurrentOrder == false) return;

        // Sells the item if completed
        if (CheckOrderCompletion(clothingItem, dry, pixels) == true)
        {
            // Destroy item and remove order for successful sale
            Destroy(item);
            Destroy(gameObject);
            OrderManager.Instance.StartNextOrder();
        }
    }

    bool CheckOrderCompletion(ClothingItem clothingItem, bool dry, byte[] pixels)
    {
        // Return true if clothes are correct, dry, and accurate >= 95%
        return clothingItem == order.ClothingItem && dry && order.getAccuracy(pixels) >= 0.95f;
    }

}
