using UnityEngine;
using UnityEngine.UI;

public class DryingRack : MonoBehaviour
{
    [SerializeField] float completionTime;
    [SerializeField] Slider progressBar;
    private float startTime;
    private float elapsedTime;
    private int lastFrameChildCount = 0;

    void Start()
    {

    }

    void Update()
    {
        if (transform.childCount == 2 && lastFrameChildCount == 1)
        {
            startTime = Time.time;
        }

        if (transform.childCount > 1)
        {
            elapsedTime = Time.time - startTime;

            if (elapsedTime >= completionTime)
            {
                // Drying Complete
                elapsedTime = 0f;
                progressBar.gameObject.SetActive(false);

                GetComponentInChildren<Clothing>().dry = true;
            }
        }

        if (elapsedTime == 0f || transform.childCount == 1)
        {
            progressBar.gameObject.SetActive(false);
        }
        else
        {
            progressBar.gameObject.SetActive(true);
            progressBar.value = Mathf.Clamp01(elapsedTime / completionTime);
        }

        lastFrameChildCount = transform.childCount;
    }
}
