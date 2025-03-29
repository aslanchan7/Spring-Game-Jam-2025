using UnityEngine;

public class TrashCanScript : MonoBehaviour
{
    public new Camera camera;
    public Sprite closedSprite;
    public Sprite openSprite;

    void Start()
    {
        camera = FindAnyObjectByType<Camera>();
    }

    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = closedSprite;
    }
}
