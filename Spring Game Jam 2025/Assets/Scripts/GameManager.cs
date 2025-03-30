using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int Money;

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

    }

    void Update()
    {

    }
}
