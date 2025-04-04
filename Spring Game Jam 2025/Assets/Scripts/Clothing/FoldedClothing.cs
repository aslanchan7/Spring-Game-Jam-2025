using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FoldedClothing : DragDroppable
{
    [SerializeField] private GameObject clothing;
    private PaintingTable table;

    void Start()
    {
    }

    public override IEnumerator DragUpdate(GameObject clickedObject)
    {
        table = FindAnyObjectByType<PaintingTable>();
        if (table.currentPaint != 0)
        {
            yield break;
        }
        OnStartDrag();
        while (InputManager.Instance.MouseClick.ReadValue<float>() != 0f)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 velocity = Vector3.zero;
            clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, mousePos, ref velocity, mouseDragSpeed);
            OnDrag();
            yield return null;
        }
        OnEndDrag();
    }

    public override void OnDrag()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "DragDroppable";

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        List<RaycastHit2D> hits = new();
        Physics2D.Raycast(mousePos, Vector2.zero, filter, hits);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Trash Can"))
            {
                hit.collider.GetComponent<SpriteRenderer>().sprite = hit.collider.GetComponent<TrashCanScript>().openSprite;
            }
        }
    }

    public override void OnEndDrag()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        List<RaycastHit2D> hits = new();
        Physics2D.Raycast(mousePos, Vector2.zero, filter, hits);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Painting Table"))
            {

                if (!hit.collider.GetComponentInChildren<DragDroppable>())
                {
                    // Instantiate the shirt prefab
                    GameObject instantiatedObject = Instantiate(clothing, hit.transform, false);
                    instantiatedObject.transform.localPosition = new Vector2(0, 1f);
                    Destroy(gameObject);
                    return;
                }
            }
            // else if (hit.collider.CompareTag("Drying Rack"))
            // {
            //     if (hit.collider.transform.childCount == 1)
            //     {
            //         // Instantiate the shirt prefab
            //         GameObject instantiatedObject = Instantiate(clothing, hit.transform, false);
            //         instantiatedObject.transform.localPosition = new Vector2(0, -0.45f);
            //         Destroy(gameObject);
            //         return;
            //     }
            // }
            else if (hit.collider.CompareTag("Trash Can"))
            {
                Destroy(gameObject);
                return;
            }
        }

        if (transform.parent.CompareTag("Painting Table"))
        {
            transform.localPosition = new Vector2(0, 1f);
            return;
        }
        else if (transform.parent.CompareTag("Drying Rack"))
        {
            transform.localPosition = new Vector2(0, -0.45f);
            return;
        }

        Destroy(gameObject);
    }
}
