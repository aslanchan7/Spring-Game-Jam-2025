using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] GameObject orderBoxPrefab;
    [SerializeField] GameObject ordersList;
    [SerializeField] float orderInterval; // # seconds between each order
    private float lastOrderTime;

    void Start()
    {

    }

    void Update()
    {
        // Randomly create orders
        if (Time.time - lastOrderTime >= orderInterval)
        {
            // Make an order
            ClothingItem clothingItem = (ClothingItem)Random.Range(0, 3);
            Pattern pattern = (Pattern)Random.Range(0, 3);
            PatternColor patternColor = (PatternColor)Random.Range(0, 3);
            Order order = new(clothingItem, pattern, patternColor);

            // Debug.Log("New Order: " + order.ClothingItem + ", " + order.Pattern + ", " + order.PatternColor);
            Instantiate(orderBoxPrefab, ordersList.transform);
            string orderPatternText = patternColor.ToString() + " " + pattern.ToString();
            if (order.Pattern == Pattern.None)
            {
                orderPatternText = "No Pattern";
            }
            orderBoxPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = clothingItem.ToString();
            orderBoxPrefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = orderPatternText;
            lastOrderTime = Time.time;
        }
    }
}
