using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class DragDroppable : MonoBehaviour, IDrag
{
    public float mouseDragSpeed = 0f;
    public ContactFilter2D filter;
    [HideInInspector] public Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void CallDragUpdate(GameObject clickedObject)
    {
        StartCoroutine(DragUpdate(clickedObject));
    }

    public virtual IEnumerator DragUpdate(GameObject clickedObject)
    {
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

    public virtual void OnStartDrag()
    {
        Debug.Log("OnStartDrag not implemented");
    }

    public virtual void OnDrag()
    {
        Debug.Log("OnDrag not implemented");
    }

    public virtual void OnEndDrag()
    {
        Debug.Log("OnEndDrag not implemented");
    }
}
