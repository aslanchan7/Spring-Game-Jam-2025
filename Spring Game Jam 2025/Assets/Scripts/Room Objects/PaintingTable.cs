using Unity.Collections;
using UnityEngine;

public class PaintingTable : MonoBehaviour
{

    public byte currentPaint = 0;

    public static bool[] allTrueStencil = new bool[64 * 64];
    [HideInInspector] public StencilScript activeStencil;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 64 * 64; i++)
        {
            allTrueStencil[i] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool[] getStencilMap()
    {
        if (activeStencil) return activeStencil.stencilMap;
        else return allTrueStencil;
    }

    public void setActiveStencil(byte id)
    {
        if (!activeStencil || activeStencil.id != id)
        {
            if (activeStencil) activeStencil.disable();
        
            StencilScript[] stencils = GetComponentsInChildren<StencilScript>();
            
            for (int i = 0; i < stencils.Length; i++)
            {
                Debug.Log(stencils[i].id + " " + id);
                if (stencils[i].id == id) {
                    Debug.Log("Found stencil");
                    stencils[i].enable();
                    activeStencil = stencils[i];
                }
            }

        }
        else if (activeStencil && activeStencil.id == id)
        {
            activeStencil.disable();
            activeStencil = null;
        }
    }

    public void removeActiveStencil()
    {
        activeStencil.disable();
        activeStencil = null;
    }
}
