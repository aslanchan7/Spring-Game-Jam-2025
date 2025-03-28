using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DraggableClothing : DragDroppable
{
    [SerializeField] GameObject clothing;

    public override void OnStartDrag()
    {
        throw new System.NotImplementedException();
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
                    instantiatedObject.transform.localPosition = Vector2.zero;
                    instantiatedObject.transform.localScale = new(0.75f, 0.75f);
                }
                else if (hit.collider.transform.GetChild(0) == this.transform)
                {
                    transform.localPosition = Vector2.zero;
                    transform.localScale = new(0.75f, 0.75f);
                    return;
                }
            }
        }

        // Delete this current dragged shirt
        Destroy(gameObject);
    }
}
