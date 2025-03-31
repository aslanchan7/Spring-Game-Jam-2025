using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    private int currentTutorial = 1;
    [SerializeField] private int totalTutorials;

    [Header("Text Settings")]
    [TextArea][SerializeField] private string[] itemInfo;
    [SerializeField] private float textSpeed = 0.01f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI itemInfoText;
    private int currentDisplayingText = 0;

    void Awake()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            transform.GetChild(currentTutorial).gameObject.SetActive(false);
            currentTutorial++;
            if (currentTutorial <= totalTutorials)
            {
                transform.GetChild(currentTutorial).gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                gameObject.SetActive(false);
            }
        }
    }
}
