using UnityEngine;

public class TrashCanScript : MonoBehaviour
{
    public new Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = FindAnyObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
