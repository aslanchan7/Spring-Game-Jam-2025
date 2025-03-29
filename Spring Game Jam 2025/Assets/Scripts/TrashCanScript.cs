using UnityEngine;

public class TrashCanScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.MouseClick.ReadValue<float>() != 0f)
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

            if (hit.collider == gameObject)
            {
                Debug.Log("Mouse is over");
            }
        }
    }
}
