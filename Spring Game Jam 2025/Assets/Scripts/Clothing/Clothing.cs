using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Clothing : DragDroppable
{
    private PaintingTable table;

    public bool dry = false;
    public DrawScript drawScript;

    public ClothingItem clothingItem;

    void Start()
    {
        table = FindAnyObjectByType<PaintingTable>();
        drawScript = GetComponent<DrawScript>();
    }

    public byte[] getPixels()
    {
        return drawScript.pixels;
    }

    public override IEnumerator DragUpdate(GameObject clickedObject)
    {
        if (table.currentPaint != 0)
        {
            yield break;
        }
        if (gameObject.transform.parent.CompareTag("Painting Table") && table.activeStencil)
        {
            table.setActiveStencil(table.activeStencil.id);
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
        // Raycast for UI Elements
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (var raycastResult in raycastResults)
        {
            raycastResult.gameObject.TryGetComponent<OrderBox>(out var orderBoxComponent);
            if (orderBoxComponent != null)
            {
                if (orderBoxComponent.isCurrentOrder)
                {
                    raycastResult.gameObject.GetComponent<RectTransform>().localScale = new(2.1f, 2.1f, 2.1f);
                }
            }
        }

        // Raycast for Physics Colliders
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        List<RaycastHit2D> hits = new();
        Physics2D.Raycast(mousePos, Vector2.zero, filter, hits);
        bool containsTrashCan = false;
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Trash Can"))
            {
                hit.collider.GetComponent<SpriteRenderer>().sprite = hit.collider.GetComponent<TrashCanScript>().openSprite;
                gameObject.GetComponent<SpriteRenderer>().color = new(1f, 1f, 1f, 0.3f);
                containsTrashCan = true;
            }
        }

        if (!containsTrashCan)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new(1f, 1f, 1f, 1f);
        }
    }

    public override void OnEndDrag()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (var raycastResult in raycastResults)
        {
            Debug.Log("Hello");
            raycastResult.gameObject.TryGetComponent<OrderBox>(out var orderBoxComponent);
            if (orderBoxComponent != null)
            {
                orderBoxComponent.TrySell(gameObject, clothingItem, dry, getPixels());
            }
        }

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        List<RaycastHit2D> hits = new();
        Physics2D.Raycast(mousePos, Vector2.zero, filter, hits);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Painting Table"))
            {
                if (!hit.collider.GetComponentInChildren<DragDroppable>() || hit.collider.transform == this.transform.parent)
                {
                    gameObject.transform.parent = hit.collider.transform;
                    transform.localPosition = new Vector2(0, 1f);
                    dry = false;
                    return;
                }
            }
            else if (hit.collider.CompareTag("Drying Rack"))
            {
                if (!hit.collider.GetComponentInChildren<DragDroppable>() || hit.collider.transform == this.transform.parent)
                {
                    gameObject.transform.parent = hit.collider.transform;
                    transform.localPosition = new Vector2(0, -0.45f);
                    return;
                }
            }
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
