using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject orderBoxPrefab;
    [SerializeField] GameObject ordersList;

    [Header("Order Settings")]
    [SerializeField] float orderInterval; // # seconds between each order
    [SerializeField] int maxOrderNum;
    private float lastOrderTime;
    private List<Order> orders = new();

    void Start()
    {

    }

    void Update()
    {
        // Randomly create orders
        if (Time.time - lastOrderTime >= orderInterval && orders.Count < maxOrderNum)
        {
            // Make an order
            Order order = GenerateRandomOrder();

            // Instantiate Order Box
            CreateOrderBox(order);

            lastOrderTime = Time.time;
            orders.Add(order);
        }
    }

    private Order GenerateRandomOrder()
    {
        ClothingItem clothingItem = (ClothingItem)Random.Range(0, 3);
        Pattern pattern = (Pattern)Random.Range(0, 3);
        PatternColor patternColor = (PatternColor)Random.Range(0, 3);
        Order order = new(clothingItem, pattern, patternColor);
        return order;
    }

    private void CreateOrderBox(Order order)
    {
        GameObject orderBox = Instantiate(orderBoxPrefab, ordersList.transform);
        string orderPatternText = order.PatternColor.ToString() + " " + order.Pattern.ToString();
        if (order.Pattern == Pattern.None)
        {
            orderPatternText = "No Pattern";
        }
        orderBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = order.ClothingItem.ToString();
        orderBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = orderPatternText;
    }
}