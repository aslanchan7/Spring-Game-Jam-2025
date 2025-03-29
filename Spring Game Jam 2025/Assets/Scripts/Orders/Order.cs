using UnityEngine;
using System.IO;

public class Order
{
    public ClothingItem ClothingItem;
    public Pattern Pattern;
    public PatternColor PatternColor;

    public Order(ClothingItem clothingItem, Pattern pattern, PatternColor patternColor)
    {
        this.ClothingItem = clothingItem;
        this.Pattern = pattern;
        this.PatternColor = patternColor;
    }

    // Generates a grid of bytes that represent a correct match
    public byte[] generatePattern()
    {
        // Get original image
        byte[] image;
        
        if (ClothingItem == ClothingItem.Shirt) image = File.ReadAllBytes("Assets/Sprites/Clothes/shirt.png");
        else if (ClothingItem == ClothingItem.Pants) image = File.ReadAllBytes("Assets/Sprites/Clothes/pants.png");
        else if (ClothingItem == ClothingItem.Hat) image = File.ReadAllBytes("Assets/Sprites/Clothes/hat.png");
        else image = File.ReadAllBytes("Assets/Sprites/Clothes/shirt.png");

        Texture2D tmpTexture = new Texture2D(64, 64);
        tmpTexture.LoadImage(image);
        Color[] clothingPixels = tmpTexture.GetPixels();

        // Generate pixel array

        byte[] pixels = new byte[64 * 64];

        for (int i = 0; i < clothingPixels.Length; i++) {
            if (clothingPixels[i] == DrawScript.black) pixels[i] = 1;
            else if (clothingPixels[i] == DrawScript.shirtWhite || clothingPixels[i] == DrawScript.pantsBlue || clothingPixels[i] == DrawScript.hatRed) pixels[i] = 32;
        }

        return pixels;
    }

    // Generates a grid of bytes that represent a correct match
    public Sprite generateSprite()
    {
        return DrawScript.generateSpriteFromPixels(generatePattern(), 64, 64);
    }

    public float getAccuracy(byte[] pixels)
    {
        byte[] patternPixels = generatePattern();
        if (patternPixels.Length != pixels.Length) return 0;
        int totalPixels = 0, correctPixels = 0;

        for (int i = 0; i < patternPixels.Length; i++) {
            if (patternPixels[i] <= 1) continue;
            else if (patternPixels[i] == pixels[i]) correctPixels++;
            totalPixels++;
        }

        return (float) correctPixels / totalPixels;
    }
}

public enum ClothingItem
{
    Shirt,
    Pants,
    Hat
}

public enum Pattern
{
    None,
    Striped,
    Clover
}

public enum PatternColor
{
    Red,
    Blue,
    Yellow
}