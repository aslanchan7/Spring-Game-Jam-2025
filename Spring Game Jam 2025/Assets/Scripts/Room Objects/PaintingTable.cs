using Unity.Collections;
using UnityEngine;

public class PaintingTable : MonoBehaviour
{

    public byte currentPaint = 0;
    public Spray spraySize = Spray.Small;

    [HideInInspector] public static bool[] allTrueStencil = new bool[64 * 64];
    [HideInInspector] public StencilScript activeStencil;

    public bool isTitleScreen = false;

    void OnEnable()
    {
        UpgradeEventManager.UpgradePaintArea += OnUpgradePaintArea;
    }

    void OnDisable()
    {
        UpgradeEventManager.UpgradePaintArea -= OnUpgradePaintArea;
    }

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
        Event current = Event.current;

        if (currentPaint > 0 && Input.GetMouseButtonUp(0) && !isTitleScreen) {
            currentPaint = 0;
        }
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
                if (stencils[i].id == id) {
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

    public void OnUpgradePaintArea()
    {
        switch (spraySize)
        {
            case Spray.Small:
                spraySize = Spray.Medium;
                break;
            case Spray.Medium:
                spraySize = Spray.Big;
                break;
            case Spray.Big:
                spraySize = Spray.Giant;
                break;
            case Spray.Giant:
                spraySize = Spray.Massive;
                break;
            default:
                break;
        }
    }
}
