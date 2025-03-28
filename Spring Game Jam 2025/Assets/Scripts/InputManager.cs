using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public InputAction MouseClick;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        MouseClick.Enable();
        MouseClick.performed += gameObject.GetComponent<DragAndDrop>().MousePressed;
    }

    private void OnDisable()
    {
        MouseClick.performed -= gameObject.GetComponent<DragAndDrop>().MousePressed;
        MouseClick.Disable();
    }
}
