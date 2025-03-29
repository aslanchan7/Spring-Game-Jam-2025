using System;
using UnityEngine;

public class StencilScript : MonoBehaviour
{
    public Sprite referenceSprite;

    public bool[] stencilMap = new bool[64 * 64];

    public byte id;
    private bool active = false;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        stencilMap = generateStencilFromSprite(referenceSprite);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enable()
    {
        spriteRenderer.enabled = true;
        active = true;
    }

    public void disable()
    {
        spriteRenderer.enabled = false;
        active = false;
    }

    public static bool[] generateStencilFromSprite(Sprite sprite)
    {
        Color[] block = sprite.texture.GetPixels((int) System.Math.Ceiling(sprite.textureRect.x), 
			                                              (int) System.Math.Ceiling(sprite.textureRect.y), 
			                                              (int) System.Math.Ceiling(sprite.textureRect.width), 
			                                              (int) System.Math.Ceiling(sprite.textureRect.height));
        return generateStencilFromColors(block);
    }

    public static bool[] generateStencilFromColors(Color[] colors)
    {
        bool[] stencilMap = new bool[64 * 64];

        for (int i = 0; i < colors.Length; i++) {
            if (colors[i].a != 1) stencilMap[i] = true;
            else stencilMap[i] = false;
        }

        return stencilMap;
    }
}
