using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    [HideInInspector] public Camera mainCamera;
    public ContactFilter2D filter;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void MousePressed(InputAction.CallbackContext context)
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        List<RaycastHit2D> hits = new();
        Physics2D.Raycast(mousePos, Vector2.zero, filter, hits);
        Debug.Log(hits.Count);
        foreach (var hit in hits)
        {
            hit.collider.TryGetComponent<DragDroppable>(out var dragDropComponent);
            if (dragDropComponent != null)
            {
                dragDropComponent.CallDragUpdate(hit.collider.gameObject);
            }
        }
    }
}
