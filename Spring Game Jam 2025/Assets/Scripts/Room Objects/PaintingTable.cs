using Unity.Collections;
using UnityEngine;

public class PaintingTable : MonoBehaviour
{

    public byte currentPaint = 0;
    public Spray spraySize = Spray.Small;

    [HideInInspector] public static bool[] allTrueStencil = new bool[64 * 64];
    [HideInInspector] public StencilScript activeStencil;

    public bool isTitleScreen = false;
    public AudioSource spraySound;

    public static bool[] smallSpray;
    public static bool[] mediumSpray;
    public static bool[] bigSpray;
    public static bool[] giantSpray;
    public static bool[] massiveSpray;

    public static bool[] topHalfStencil;
    public static bool[] leftHalfStencil;
    public static bool[] sleevesStencil;
    public static bool[] squiggleStencil;
    public static bool[] spotsStencil;
    public static bool[] beeStencil;
    public static bool[] shamrockStencil;
    public static bool[] unicycleStencil;
    public static bool[] shirtStencil;
    public static Color[] shirtSprite;

    void Awake()
    {
        smallSpray = generateBrushFromColors(Resources.Load<Sprite>("Sprites/Brushes/small_spray").texture.GetPixels());
        mediumSpray = generateBrushFromColors(Resources.Load<Sprite>("Sprites/Brushes/medium_spray").texture.GetPixels());
        bigSpray = generateBrushFromColors(Resources.Load<Sprite>("Sprites/Brushes/big_spray").texture.GetPixels());
        giantSpray = generateBrushFromColors(Resources.Load<Sprite>("Sprites/Brushes/giant_spray").texture.GetPixels());
        massiveSpray = generateBrushFromColors(Resources.Load<Sprite>("Sprites/Brushes/massive_spray").texture.GetPixels());

        topHalfStencil = StencilScript.generateStencilFromColors(Resources.Load<Sprite>("Sprites/Stencils/top_half_stencil").texture.GetPixels());
        leftHalfStencil = StencilScript.generateStencilFromColors(Resources.Load<Sprite>("Sprites/Stencils/left_half_stencil").texture.GetPixels());
        sleevesStencil = StencilScript.generateStencilFromColors(Resources.Load<Sprite>("Sprites/Stencils/sleeve_stencil").texture.GetPixels());
        squiggleStencil = StencilScript.generateStencilFromColors(Resources.Load<Sprite>("Sprites/Stencils/squiggle_stencil").texture.GetPixels());
        spotsStencil = StencilScript.generateStencilFromColors(Resources.Load<Sprite>("Sprites/Stencils/spots_stencil").texture.GetPixels());
        beeStencil = StencilScript.generateStencilFromColors(Resources.Load<Sprite>("Sprites/Stencils/bee_stencil").texture.GetPixels());
        shamrockStencil = StencilScript.generateStencilFromColors(Resources.Load<Sprite>("Sprites/Stencils/shamrock_stencil").texture.GetPixels());
        unicycleStencil = StencilScript.generateStencilFromColors(Resources.Load<Sprite>("Sprites/Stencils/unicycle_stencil").texture.GetPixels());
        shirtStencil = StencilScript.generateStencilFromColors(Resources.Load<Sprite>("Sprites/Clothes/shirt").texture.GetPixels());
        
        shirtSprite = Resources.Load<Sprite>("Sprites/Clothes/shirt").texture.GetPixels();
    }

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
        this.spraySound = GetComponent<AudioSource>();
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

        if (spraySound)
        {
            if (currentPaint == 0)
            {
                spraySound.mute = true;
            }
            else
            {
                spraySound.mute = false;
            }
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

    public static bool[] generateBrushFromColors(Color[] colors)
    {
        bool[] output = new bool[colors.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            output[i] = colors[i].a == 1;
        }

        return output;
    }
}