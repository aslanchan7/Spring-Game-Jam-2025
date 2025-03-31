using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawScript : MonoBehaviour
{
    private new Camera camera;
    private PaintingTable table;

    public SpriteRenderer spriteRenderer;
    public Sprite referenceSprite;

    private int width, height;

    [HideInInspector] public byte[] pixels;

    private Vector2 lastMousePosition = new Vector2(0, 0);
    private bool mouseLastPressed = false;

    public bool applyTableStencil = true;
    public bool applyShirtStencil = false;
    public bool completeWhenFilled = false;

    public Animator flashAnimator;

    [HideInInspector] public static readonly Color black = Color.black; // 0
    [HideInInspector] public static readonly Color transparent = Color.clear; // 1
    [HideInInspector] public static readonly Color shadow = new Color32(0, 0, 0, 128); // 2

    [HideInInspector] public static readonly Color shirtWhite = Color.white; // 4
    [HideInInspector] public static readonly Color pantsBlue = new Color32(63, 63, 116, 255); // 5
    [HideInInspector] public static readonly Color hatRed = new Color32(217, 87, 99, 255); // 6
    
    [HideInInspector] public static readonly Color stencilYellow = new Color32(251, 242, 54, 255); // 10
    [HideInInspector] public static readonly Color stencilOrange = new Color32(223, 113, 38, 255); // 11
    [HideInInspector] public static readonly Color tableBrown = new Color32(143, 86, 59, 255); // 12
    
    

    [HideInInspector] public static readonly Color sprayGreen = new Color32(106, 190, 48, 255); // 32
    [HideInInspector] public static readonly Color sprayRed = new Color32(172, 50, 50, 255); // 33
    [HideInInspector] public static readonly Color sprayBlue = new Color32(99, 155, 255, 255); // 34
    [HideInInspector] public static readonly Color sprayPurple = new Color32(151, 0, 160, 255); // 35

    [HideInInspector] public static bool[] smallSpray, mediumSpray, bigSpray, giantSpray, massiveSpray;

    private static bool[] currentSpray;

    private bool hasUpdated;
    public AudioSource completeSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = FindFirstObjectByType<Camera>();
        table = FindFirstObjectByType<PaintingTable>();

        Color[] block = referenceSprite.texture.GetPixels((int) Math.Ceiling(referenceSprite.textureRect.x), 
			                                              (int) Math.Ceiling(referenceSprite.textureRect.y), 
			                                              (int) Math.Ceiling(referenceSprite.textureRect.width), 
			                                              (int) Math.Ceiling(referenceSprite.textureRect.height));
        width = (int) Math.Ceiling(referenceSprite.textureRect.width);
        height = (int) Math.Ceiling(referenceSprite.textureRect.height);
        pixels = new byte[width * height];
        for (int i = 0; i < block.Length; i++) {
            if (block[i].a == 0) pixels[i] = 0;
            else if (block[i] == black) pixels[i] = 1;
            else if (block[i] == shadow) pixels[i] = 2;

            else if (block[i] == shirtWhite) pixels[i] = 4;
            else if (block[i] == pantsBlue) pixels[i] = 5;
            else if (block[i] == hatRed) pixels[i] = 6;

            else if (block[i] == stencilYellow) pixels[i] = 10;
            else if (block[i] == stencilOrange) pixels[i] = 11;
            else if (block[i] == tableBrown) pixels[i] = 12;

            else if (block[i] == sprayGreen) pixels[i] = 32;
            else if (block[i] == sprayRed) pixels[i] = 33;
            else if (block[i] == sprayBlue) pixels[i] = 34;
            else if (block[i] == sprayPurple) pixels[i] = 35;
            else pixels[i] = 0;
        }

        smallSpray = generateBrushFromFile(Application.streamingAssetsPath + "/Sprites/Brushes/small_spray.png");
        mediumSpray = generateBrushFromFile(Application.streamingAssetsPath + "/Sprites/Brushes/medium_spray.png");
        bigSpray = generateBrushFromFile(Application.streamingAssetsPath + "/Sprites/Brushes/big_spray.png");
        giantSpray = generateBrushFromFile(Application.streamingAssetsPath + "/Sprites/Brushes/giant_spray.png");
        massiveSpray = generateBrushFromFile(Application.streamingAssetsPath + "/Sprites/Brushes/massive_spray.png");
    }

    // Update is called once per frame
    void Update()
    {
        if (table.spraySize == Spray.Small)
        {
            currentSpray = smallSpray;
        }
        else if (table.spraySize == Spray.Medium)
        {
            currentSpray = mediumSpray;
        }
        else if (table.spraySize == Spray.Big)
        {
            currentSpray = bigSpray;
        }
        else if (table.spraySize == Spray.Giant)
        {
            currentSpray = giantSpray;
        }
        else if (table.spraySize == Spray.Massive)
        {
            currentSpray = massiveSpray;
        }
        paintWithMouse();
    }

    // Generates a sprite based on the pixel array
    public static Sprite generateSpriteFromPixels(byte[] pixels, int width, int height)
    {
        Texture2D tmpTexture = new Texture2D(width, height);

        for (int i = 0; i < pixels.Length; i++) {
            if (pixels[i] == 0) tmpTexture.SetPixel(i % width, i / width, transparent);
            else if (pixels[i] == 1) tmpTexture.SetPixel(i % width, i / width, black);
            else if (pixels[i] == 2) tmpTexture.SetPixel(i % width, i / width, shadow);

            else if (pixels[i] == 4) tmpTexture.SetPixel(i % width, i / width, shirtWhite);
            else if (pixels[i] == 5) tmpTexture.SetPixel(i % width, i / width, pantsBlue);
            else if (pixels[i] == 6) tmpTexture.SetPixel(i % width, i / width, hatRed);

            else if (pixels[i] == 10) tmpTexture.SetPixel(i % width, i / width, stencilYellow);
            else if (pixels[i] == 11) tmpTexture.SetPixel(i % width, i / width, stencilOrange);
            else if (pixels[i] == 12) tmpTexture.SetPixel(i % width, i / width, tableBrown);

            else if (pixels[i] == 32) tmpTexture.SetPixel(i % width, i / width, sprayGreen);
            else if (pixels[i] == 33) tmpTexture.SetPixel(i % width, i / width, sprayRed);
            else if (pixels[i] == 34) tmpTexture.SetPixel(i % width, i / width, sprayBlue);
            else if (pixels[i] == 35) tmpTexture.SetPixel(i % width, i / width, sprayPurple);
        }

        tmpTexture.filterMode = FilterMode.Point;
        tmpTexture.Apply(false, true);

        return Sprite.Create(tmpTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 10);
    }

    private void updateSprite()
    {
        if (spriteRenderer) spriteRenderer.sprite = generateSpriteFromPixels(pixels, width, height);
    }

    void paintWithMouse() {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.isPressed && gameObject.transform.parent.CompareTag("Painting Table") && table.currentPaint != 0 && spriteRenderer.enabled)
        {
            Vector2 mousePosition = mouse.position.ReadValue();
            mousePosition = camera.ScreenToWorldPoint(mousePosition);
            mousePosition -= new Vector2(transform.position.x, transform.position.y);
            mousePosition /= new Vector2(width / 10f, height / 10f);
            mousePosition += new Vector2(0.5f, 0.5f);
            if (mouseLastPressed) {
                Vector2 mouseDirection = (mousePosition - lastMousePosition).normalized;
                float mouseDistance = (mousePosition - lastMousePosition).magnitude;
                if (mouseDistance >= 1f / width) {
                    for (float i = 0; i < mouseDistance; i += 1f / width) {
                        brushPosition(lastMousePosition + (mouseDirection * i), table.currentPaint);
                    }
                }
            }
            brushPosition(mousePosition, table.currentPaint);
            lastMousePosition = mousePosition;
            mouseLastPressed = true;
            if (hasUpdated)
            {
                if (completeWhenFilled)
                {
                    float completion = getStencilCompletionForColor(table.currentPaint);
                    if (completion >= 0.93 && completion < 1) {
                        completeStencil(table.currentPaint);
                        flashAnimator.Play("ShirtFlash");
                        flashAnimator.Play("ShirtIdle");
                        if (completeSound) completeSound.Play();
                    }
                }
                updateSprite();
                hasUpdated = false;
            }
        } else {
            mouseLastPressed = false;
        }
    }

    void brushPosition(Vector2 mousePosition, byte color) {
        bool[] stencil = table.getStencilMap();
        if (mousePosition.x >= -0.5 && mousePosition.x < 1.5 && mousePosition.y >= -0.5 && mousePosition.y < 1.5) {
            int pixelX = (int) (mousePosition.x * width);
            int pixelY = (int) (mousePosition.y * height);
            int brushSize = (int) Math.Sqrt(currentSpray.Length);
            for (int i = 0; i < currentSpray.Length; i++) {
                if (!currentSpray[i]) continue;
                int newPixelX = pixelX - (brushSize / 2) + (i % brushSize);
                int newPixelY = pixelY - (brushSize / 2) + (i / brushSize);
                paintPixel(newPixelX, newPixelY, color, stencil);
            }
        }
    }

    void paintPixel(int x, int y, byte color, bool[] stencil) {
        if (x < 0 || x >= width || y < 0 || y >= height) return;
        if (applyTableStencil && !stencil[x + (y * width)]) return;
        if (applyShirtStencil && table.GetComponentInChildren<Clothing>() && !Order.shirtStencil[x + (y * width)]) return;
        int pixel = x + (y * width);
        if (pixels[pixel] >= 4) {
            if (pixels[pixel] == color) return;
            hasUpdated = true;
            pixels[pixel] = color;
        }
    }

    public static Color[] getColorsFromFile(string file)
    {
        byte[] image = File.ReadAllBytes(file);

        Texture2D tmpTexture = new Texture2D(64, 64);
        tmpTexture.LoadImage(image);
        Color[] pixels = tmpTexture.GetPixels();

        return pixels;
    }

    public static bool[] generateBrushFromFile(string file)
    {
        Color[] colors = getColorsFromFile(file);

        bool[] output = new bool[colors.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            output[i] = colors[i].a == 1;
        }

        return output;
    }

    public float getStencilCompletionForColor(byte color)
    {
        bool[] stencil = table.getStencilMap();
        int total = 0;
        int correct = 0;

        for (int i = 0; i < pixels.Length; i++)
        {
            if (!stencil[i]) continue;
            else if (pixels[i] < 4) continue;
            else
            {
                total++;
                if (pixels[i] == color) correct++;
            }
        }

        return (float) correct / total;
    }

    public void completeStencil(byte color)
    {
        bool[] stencil = table.getStencilMap();

        for (int i = 0; i < pixels.Length; i++)
        {
            if (!stencil[i]) continue;
            else if (pixels[i] < 4) continue;
            else
            {
                pixels[i] = color;
            }
        }
    }
}

public enum Spray
{
    Small, Medium, Big, Giant, Massive
}
