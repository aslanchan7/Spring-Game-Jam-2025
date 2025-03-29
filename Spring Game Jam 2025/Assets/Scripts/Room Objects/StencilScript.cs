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

        Color[] block = referenceSprite.texture.GetPixels((int) System.Math.Ceiling(referenceSprite.textureRect.x), 
			                                              (int) System.Math.Ceiling(referenceSprite.textureRect.y), 
			                                              (int) System.Math.Ceiling(referenceSprite.textureRect.width), 
			                                              (int) System.Math.Ceiling(referenceSprite.textureRect.height));
        for (int i = 0; i < block.Length; i++) {
            if (block[i].a != 1) stencilMap[i] = true;
            else stencilMap[i] = false;
        }
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
}
