using UnityEngine;
using UnityEngine.InputSystem;

public class DrawScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private new Camera camera;

    public Sprite originalSprite;

    private static int width, height;

    private byte[] pixels;

    private Vector2 lastMousePosition = new Vector2(0, 0);
    private bool mouseLastPressed = false;

    private PaintingTable table;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = FindAnyObjectByType<Camera>();
        Color[] block = originalSprite.texture.GetPixels((int) System.Math.Ceiling(originalSprite.textureRect.x), 
			                                             (int) System.Math.Ceiling(originalSprite.textureRect.y), 
			                                             (int) System.Math.Ceiling(originalSprite.textureRect.width), 
			                                             (int) System.Math.Ceiling(originalSprite.textureRect.height));
        width = (int) System.Math.Ceiling(originalSprite.textureRect.width);
        height = (int) System.Math.Ceiling(originalSprite.textureRect.height);
        pixels = new byte[width * height];
        for (int i = 0; i < block.Length; i++) {
            if (block[i].a == 0) pixels[i] = 0;
            else if (block[i] == Color.black) pixels[i] = 1;
            else pixels[i] = 2;
        }
        table = FindAnyObjectByType<PaintingTable>();
    }

    // Update is called once per frame
    void Update()
    {
        Texture2D tmpTexture = new Texture2D(width, height);

        for (int i = 0; i < pixels.Length; i++) {
            if (pixels[i] == 0) tmpTexture.SetPixel(i % width, i / width, Color.clear);
            else if (pixels[i] == 1) tmpTexture.SetPixel(i % width, i / width, Color.black);
            else if (pixels[i] == 2) tmpTexture.SetPixel(i % width, i / width, Color.white);
            else if (pixels[i] == 3) tmpTexture.SetPixel(i % width, i / width, new Color32(106, 190, 48, 255));
            else if (pixels[i] == 4) tmpTexture.SetPixel(i % width, i / width, new Color32(172, 50, 50, 255));
            else if (pixels[i] == 5) tmpTexture.SetPixel(i % width, i / width, new Color32(99, 155, 255, 255));
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            for (int i = 0; i < pixels.Length; i++) {
                if (pixels[i] >= 2) pixels[i] = 2;
            }
        }

        tmpTexture.filterMode = FilterMode.Point;
        tmpTexture.Apply(false, true);

        spriteRenderer.sprite = Sprite.Create(tmpTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 10);
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
        if (table.currentPaint != 0) {
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
            } else {
                mouseLastPressed = false;
            }
        }
    }

    void brushPosition(Vector2 mousePosition, byte color) {
        if (mousePosition.x >= 0 && mousePosition.x < 1 && mousePosition.y >= 0 && mousePosition.y < 1) {
            int pixelX = (int) (mousePosition.x * width);
            int pixelY = (int) (mousePosition.y * height);
            for (int i = 0; i < 49; i++) {
                if (i == 0 || i == 1 || i == 5 || i == 6 || i == 7 || i == 13 || i == 35 || i == 41 || i == 42 || i == 43 || i == 47 || i == 48) continue;
                int newPixelX = pixelX - 3 + (i % 7);
                int newPixelY = pixelY - 3 + (i / 7);
                paintPixel(newPixelX, newPixelY, color);
            }
        }
    }

    void paintPixel(int x, int y, byte color) {
        if (x < 0 || x >= width || y < 0 || y >= height) return;
        int pixel = x + (y * width);
        if (pixels[pixel] >= 2) pixels[pixel] = color;
    }
}
