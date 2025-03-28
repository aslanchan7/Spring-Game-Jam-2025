using UnityEngine;

public class DryingRack : MonoBehaviour
{
    [SerializeField] float completionTime;
    private float startTime;
    private float elapsedTime;
    private int lastFrameChildCount = 0;

    void Start()
    {

    }

    void Update()
    {
        if (transform.childCount == 1 && lastFrameChildCount == 0)
        {
            startTime = Time.time;
        }

        if (transform.childCount != 0)
        {
            elapsedTime = Time.time - startTime;

            if (elapsedTime >= completionTime)
            {
                // Drying Complete
                Debug.Log("Drying Complete");

            }
        }

        Debug.Log(elapsedTime);

        lastFrameChildCount = transform.childCount;
    }
}
