using UnityEngine;
using UnityEngine.InputSystem;

public class ClothingBox : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] GameObject foldedClothing;
    private bool clothingInstantiated;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (transform.childCount != 0) return;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Clothing Box"))
            {
                GameObject instantiatedObject = Instantiate(foldedClothing, transform, true);
                instantiatedObject.transform.position = mousePos;
                instantiatedObject.GetComponent<DragDroppable>().CallDragUpdate(instantiatedObject);
            }
        }
    }
}
