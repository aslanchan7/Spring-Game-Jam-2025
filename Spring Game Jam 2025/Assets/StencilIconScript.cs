using UnityEngine;

public class StencilIconScript : MonoBehaviour
{
    public byte id;
    private PaintingTable table;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        table = FindAnyObjectByType<PaintingTable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if ((!table.activeStencil || table.activeStencil.id != id) && !table.GetComponentInChildren<Clothing>()) return;
        table.setActiveStencil(id);
    }
}
