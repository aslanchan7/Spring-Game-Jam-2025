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
                    instantiatedObject.transform.localPosition = new Vector2(0, 1.4f);
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
                }
                else if (hit.collider.transform.GetChild(1) == this.transform)
                {
                    transform.localPosition = new Vector2(0, -0.45f);
                    return;
                }
            }
        }

        // Delete this current dragged shirt
        Destroy(gameObject);
    }
}
