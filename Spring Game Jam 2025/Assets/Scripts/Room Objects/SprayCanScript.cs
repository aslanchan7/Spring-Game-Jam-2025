using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class SprayCanScript : DragDroppable
{

    public byte color;
    public bool active;

    private PaintingTable table;

    private Vector2 startPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        table = FindAnyObjectByType<PaintingTable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active && table.currentPaint != color)
        {
            setInactive();
        }
    }

    // void OnMouseDown()
    // {
    //     if (table.currentPaint == color)
    //     {
    //         table.currentPaint = 0;
    //         setInactive();
    //     }
    //     else
    //     {
    //         table.currentPaint = color;
    //         setActive();
    //     }
    // }

    void setInactive()
    {
        transform.position = startPosition;
        active = false;
    }

    void setActive()
    {
        transform.position = startPosition + new Vector2(0, 0.3f);
        active = true;
    }

    public override void OnStartDrag()
    {
        // Rotate the game object
        transform.localRotation = Quaternion.Euler(new(0, 0, 45f));
        table.currentPaint = color;
        active = true;
    }

    public override void OnEndDrag()
    {
        table.currentPaint = 0;
        setInactive();

        // Rotate back to normal
        transform.localRotation = Quaternion.identity;
    }
}
