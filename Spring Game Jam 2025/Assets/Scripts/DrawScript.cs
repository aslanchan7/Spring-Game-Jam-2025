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

    [HideInInspector] public static readonly Color black = Color.black; // 0
    [HideInInspector] public static readonly Color transparent = Color.clear; // 1

    [HideInInspector] public static readonly Color shirtWhite = Color.white; // 4
    [HideInInspector] public static readonly Color pantsBlue = new Color32(63, 63, 116, 255); // 5
    [HideInInspector] public static readonly Color hatRed = new Color32(217, 87, 99, 255); // 6

    [HideInInspector] public static readonly Color sprayGreen = new Color32(106, 190, 48, 255); // 32
    [HideInInspector] public static readonly Color sprayRed = new Color32(172, 50, 50, 255); // 33
    [HideInInspector] public static readonly Color sprayBlue = new Color32(99, 155, 255, 255); // 34


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = FindFirstObjectByType<Camera>();
        table = FindFirstObjectByType<PaintingTable>();

        Color[] block = referenceSprite.texture.GetPixels((int) System.Math.Ceiling(referenceSprite.textureRect.x), 
			                                              (int) System.Math.Ceiling(referenceSprite.textureRect.y), 
			                                              (int) System.Math.Ceiling(referenceSprite.textureRect.width), 
			                                              (int) System.Math.Ceiling(referenceSprite.textureRect.height));
        width = (int) System.Math.Ceiling(referenceSprite.textureRect.width);
        height = (int) System.Math.Ceiling(referenceSprite.textureRect.height);
        pixels = new byte[width * height];
        for (int i = 0; i < block.Length; i++) {
            if (block[i].a == 0) pixels[i] = 0;
            else if (block[i] == black) pixels[i] = 1;
            else if (block[i] == shirtWhite) pixels[i] = 4;
            else if (block[i] == pantsBlue) pixels[i] = 5;
            else if (block[i] == hatRed) pixels[i] = 6;
            else pixels[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Generates a sprite based on the pixel array
    public static Sprite generateSpriteFromPixels(byte[] pixels, int width, int height)
    {
        Texture2D tmpTexture = new Texture2D(width, height);

        for (int i = 0; i < pixels.Length; i++) {
            if (pixels[i] == 0) tmpTexture.SetPixel(i % width, i / width, transparent);
            else if (pixels[i] == 1) tmpTexture.SetPixel(i % width, i / width, black);

            else if (pixels[i] == 4) tmpTexture.SetPixel(i % width, i / width, shirtWhite);
            else if (pixels[i] == 5) tmpTexture.SetPixel(i % width, i / width, pantsBlue);
            else if (pixels[i] == 6) tmpTexture.SetPixel(i % width, i / width, hatRed);

            else if (pixels[i] == 32) tmpTexture.SetPixel(i % width, i / width, new Color32(106, 190, 48, 255));
            else if (pixels[i] == 33) tmpTexture.SetPixel(i % width, i / width, new Color32(172, 50, 50, 255));
            else if (pixels[i] == 34) tmpTexture.SetPixel(i % width, i / width, new Color32(99, 155, 255, 255));
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            for (int i = 0; i < pixels.Length; i++) {
                if (pixels[i] >= 2) pixels[i] = 2;
            }
        }

        tmpTexture.filterMode = FilterMode.Point;
        tmpTexture.Apply(false, true);

        return Sprite.Create(tmpTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 10);
    }

    private void updateSprite()
    {
        if (spriteRenderer) spriteRenderer.sprite = generateSpriteFromPixels(pixels, width, height);
    }

    void OnMouseOver()
    {
        paintWithMouse();
    }

    void OnMouseExit()
    {
        paintWithMouse();
        mouseLastPressed = false;
    }

    void paintWithMouse() {
        if (!gameObject.transform.parent.CompareTag("Painting Table")) return;
        if (table.currentPaint == 0) return;
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.isPressed)
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
            updateSprite();
        } else {
            mouseLastPressed = false;
        }
    }

    void brushPosition(Vector2 mousePosition, byte color) {
        bool[] stencil = table.getStencilMap();
        if (mousePosition.x >= 0 && mousePosition.x < 1 && mousePosition.y >= 0 && mousePosition.y < 1) {
            int pixelX = (int) (mousePosition.x * width);
            int pixelY = (int) (mousePosition.y * height);
            for (int i = 0; i < 49; i++) {
                if (i == 0 || i == 1 || i == 5 || i == 6 || i == 7 || i == 13 || i == 35 || i == 41 || i == 42 || i == 43 || i == 47 || i == 48) continue;
                int newPixelX = pixelX - 3 + (i % 7);
                int newPixelY = pixelY - 3 + (i / 7);
                paintPixel(newPixelX, newPixelY, color, stencil);
            }
        }
    }

    void paintPixel(int x, int y, byte color, bool[] stencil) {
        if (x < 0 || x >= width || y < 0 || y >= height) return;
        if (applyTableStencil && !stencil[x + (y * width)]) return;
        int pixel = x + (y * width);
        if (pixels[pixel] >= 2) pixels[pixel] = color;
    }
}
