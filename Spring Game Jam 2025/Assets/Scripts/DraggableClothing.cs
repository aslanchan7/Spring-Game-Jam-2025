using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableClothing : DragDroppable
{
    [SerializeField] GameObject clothing;

    public override void OnDrag()
    {
        PointerEventData pointerEventData = new(EventSystem.current);
        List<RaycastResult> raycasts = new();
        EventSystem.current.RaycastAll(pointerEventData, raycasts);
        // Debug.Log(raycasts.Count);
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

                if (hit.collider.transform.childCount == 0)
                {
                    // Instantiate the shirt prefab
                    GameObject instantiatedObject = Instantiate(clothing, hit.transform, false);
                    instantiatedObject.transform.localPosition = new Vector2(0, 1.4f);
                    Destroy(gameObject);
                    return;
                }
                else if (hit.collider.transform.GetChild(0) == this.transform)
                {
                    transform.localPosition = new Vector2(0, 1.4f);
                    return;
                }
            }
            else if (hit.collider.CompareTag("Drying Rack"))
            {
                if (hit.collider.transform.childCount == 1)
                {
                    // Instantiate the shirt prefab
                    GameObject instantiatedObject = Instantiate(clothing, hit.transform, false);
                    instantiatedObject.transform.localPosition = new Vector2(0, -0.45f);
                    Destroy(gameObject);
                    return;
                }
                else if (hit.collider.transform.GetChild(1) == this.transform)
                {
                    transform.localPosition = new Vector2(0, -0.45f);
                    return;
                }
            }
        }

        if (transform.parent.CompareTag("Painting Table"))
        {
            transform.localPosition = new Vector2(0, 1.4f);
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
