using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("References")]
    [SerializeField] GameObject orderBoxPrefab;
    [SerializeField] GameObject ordersList;

    [Header("Order Settings")]
    [SerializeField] float orderInterval; // # seconds between each order
    [SerializeField] int maxOrderNum;
    [SerializeField] int[] sellPrices;
    private int stencilUpgradeIndex;
    private float timeToNextOrder;
    [HideInInspector] public OrderBox currentOrder;
    [HideInInspector] public Queue<OrderBox> nextOrders = new();

    public StencilManager stencilManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        stencilUpgradeIndex = 0;
        stencilManager = FindAnyObjectByType<StencilManager>();
    }

    void Update()
    {
        // Randomly create orders
        if (timeToNextOrder <= 0f)
        {
            // Make an order
            Order order = GenerateRandomOrder();

            // Instantiate Order Box
            OrderBox orderBox = CreateOrderBox(order);

            timeToNextOrder = orderInterval;

            nextOrders.Enqueue(orderBox);
        }

        if (nextOrders.Count + 1 < maxOrderNum)
        {
            timeToNextOrder -= Time.deltaTime;
        }

        if (currentOrder == null)
        {
            StartNextOrder();
        }
    }

    private Order GenerateRandomOrder()
    {
        Order order;

        int imprintCount = Random.Range(0, System.Math.Min(stencilManager.ActiveStencils + 1, 3));

        do
        {

            ClothingItem clothingItem = ClothingItem.Shirt;
            Imprint[] imprints = new Imprint[imprintCount];

            HashSet<Pattern> patternsUsed = new HashSet<Pattern>();
            HashSet<PatternColor> colorsUsed = new HashSet<PatternColor>();

            PatternColor baseColor = (PatternColor)Random.Range(0, 3);


            colorsUsed.Add(baseColor);

            for (int i = 0; i < imprints.Length; i++)
            {
                Pattern pattern;
                do
                {
                    pattern = (Pattern)Random.Range(0, stencilManager.ActiveStencils);
                } while (patternsUsed.Contains(pattern));

                PatternColor patternColor;
                do
                {
                    patternColor = (PatternColor)Random.Range(0, 3);
                } while (colorsUsed.Contains(patternColor));

                imprints[i] = new Imprint(patternColor, pattern);

                colorsUsed.Add(patternColor);
                patternsUsed.Add(pattern);
            }

            stencilUpgradeIndex = FindFirstObjectByType<StencilManager>().ActiveStencils - 1;
            int sellPrice = sellPrices[stencilUpgradeIndex];

            order = new(clothingItem, baseColor, imprints, sellPrice);

        } while (!order.containsGreen());

        return order;
    }

    private OrderBox CreateOrderBox(Order order)
    {
        GameObject orderBox = Instantiate(orderBoxPrefab, ordersList.transform);
        orderBox.GetComponent<OrderBox>().order = order;

        // Adjust size & color of the background box/panel
        orderBox.GetComponent<RectTransform>().sizeDelta = new(150, 135);
        Color panelColor = orderBox.GetComponent<Image>().color;
        orderBox.GetComponent<Image>().color = new(panelColor.r, panelColor.g, panelColor.b, 0.5f);

        // Adjust alpha of the image
        Color spriteColor = orderBox.transform.GetChild(0).GetComponent<Image>().color;
        orderBox.transform.GetChild(0).GetComponent<Image>().color = new(spriteColor.r, spriteColor.g,
            spriteColor.b, 0.5f);

        // Hide the timer text
        orderBox.transform.GetChild(1).gameObject.SetActive(false);

        return orderBox.GetComponent<OrderBox>();
    }

    public void StartNextOrder()
    {
        if (nextOrders.Count == 0) return;

        currentOrder = nextOrders.Dequeue();

        // Change the OrderBox settings for this new current order
        // Adjust size & color of the background box/panel
        currentOrder.GetComponent<RectTransform>().sizeDelta = new(150, 150);
        Color panelColor = currentOrder.GetComponent<Image>().color;
        currentOrder.GetComponent<Image>().color = new(panelColor.r, panelColor.g, panelColor.b, 0.8f);

        // Adjust alpha of the image
        Color spriteColor = currentOrder.transform.GetChild(0).GetComponent<Image>().color;
        currentOrder.transform.GetChild(0).GetComponent<Image>().color = new(spriteColor.r, spriteColor.g,
            spriteColor.b, 1f);

        // Hide the timer text
        currentOrder.transform.GetChild(1).gameObject.SetActive(true);

        // Start timer
        TimerEventManager.OnTimerStart();
    }
}