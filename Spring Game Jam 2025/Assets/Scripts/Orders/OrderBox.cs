using UnityEngine;
using UnityEngine.UI;

public class OrderBox : MonoBehaviour
{
    [HideInInspector] public Order order;
    [HideInInspector] public OrderManager orderManager;
    [HideInInspector] public bool isCurrentOrder;
    public Image background;
    public Image image;
    public Timer timer;
    private Animator animator;
    public AudioSource completeOrderSound;
    public AudioSource failOrderSound;

    void Start()
    {
        Sprite orderSprite = order.generateSprite();
        image.sprite = orderSprite;
        animator = GetComponent<Animator>();
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

        if (isCurrentOrder && timer.TimeToDisplay <= 0f)
        {
            DeleteOrder();
            failOrderSound.Play();
        }

        GetComponent<RectTransform>().localScale = Vector3.one * 2; // this sets the localScale to 1 when mouse is not hovering over it
    }

    public void TrySell(GameObject item, ClothingItem clothingItem, bool dry, byte[] pixels)
    {
        if (isCurrentOrder == false) return;

        // Sells the item if completed
        if (CheckOrderCompletion(clothingItem, dry, pixels) == true)
        {
            // Destroy item and remove order for successful sale
            GameManager.Instance.UpdateMoney(order.SellPrice);
            Destroy(item);
            animator.SetTrigger("Exit");
            completeOrderSound.Play();
        }
    }

    bool CheckOrderCompletion(ClothingItem clothingItem, bool dry, byte[] pixels)
    {
        // Return true if clothes are correct, dry, and accurate >= 95%
        return clothingItem == order.ClothingItem && dry && order.getAccuracy(pixels) >= 0.95f;
    }

    private void DeleteOrder()
    {
        Destroy(gameObject);
        OrderManager.Instance.StartNextOrder();
    }
}
