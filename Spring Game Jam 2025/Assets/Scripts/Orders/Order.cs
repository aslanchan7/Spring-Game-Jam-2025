using UnityEngine;
using System.IO;

public class Order
{
    public ClothingItem ClothingItem;
    public Imprint[] Imprints;
    public PatternColor BaseColor;
    public int SellPrice;

    private static readonly bool[] topHalfStencil = StencilScript.generateStencilFromColors(getColorsFromFile("Assets/Sprites/Stencils/top_half_stencil.png"));
    private static readonly bool[] leftHalfStencil = StencilScript.generateStencilFromColors(getColorsFromFile("Assets/Sprites/Stencils/left_half_stencil.png"));
    private static readonly bool[] sleevesStencil = StencilScript.generateStencilFromColors(getColorsFromFile("Assets/Sprites/Stencils/sleeve_stencil.png"));
    private static readonly bool[] squiggleStencil = StencilScript.generateStencilFromColors(getColorsFromFile("Assets/Sprites/Stencils/squiggle_stencil.png"));
    private static readonly bool[] spotsStencil = StencilScript.generateStencilFromColors(getColorsFromFile("Assets/Sprites/Stencils/spots_stencil.png"));
    private static readonly bool[] beeStencil = StencilScript.generateStencilFromColors(getColorsFromFile("Assets/Sprites/Stencils/bee_stencil.png"));
    private static readonly bool[] shamrockStencil = StencilScript.generateStencilFromColors(getColorsFromFile("Assets/Sprites/Stencils/shamrock_stencil.png"));
    private static readonly bool[] unicycleStencil = StencilScript.generateStencilFromColors(getColorsFromFile("Assets/Sprites/Stencils/unicycle_stencil.png"));
    public static readonly bool[] shirtStencil = StencilScript.generateStencilFromColors(getColorsFromFile("Assets/Sprites/Clothes/shirt.png"));

    public Order(ClothingItem clothingItem, PatternColor baseColor, Imprint[] imprints, int sellPrice)
    {
        this.ClothingItem = clothingItem;
        this.BaseColor = baseColor;
        this.Imprints = imprints;
        this.SellPrice = sellPrice;
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

        for (int i = 0; i < clothingPixels.Length; i++)
        {
            if (clothingPixels[i] == DrawScript.black) pixels[i] = 1;
            else if (clothingPixels[i] == DrawScript.shirtWhite || clothingPixels[i] == DrawScript.pantsBlue || clothingPixels[i] == DrawScript.hatRed) pixels[i] = getColorCodeFromPatternColor(BaseColor);
        }

        for (int i = 0; i < Imprints.Length; i++)
        {
            Pattern pattern = Imprints[i].Pattern;
            PatternColor patternColor = Imprints[i].PatternColor;

            bool[] stencil = getStencilFromPattern(pattern);
            byte colorcode = getColorCodeFromPatternColor(patternColor);

            for (int j = 0; j < stencil.Length; j++)
            {
                if (!stencil[j]) continue;
                if (clothingPixels[j] == DrawScript.black || clothingPixels[j] == DrawScript.transparent) continue;
                else pixels[j] = colorcode;
            }
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

        for (int i = 0; i < patternPixels.Length; i++)
        {
            if (patternPixels[i] <= 1) continue;
            else if (patternPixels[i] == pixels[i]) correctPixels++;
            totalPixels++;
        }

        return (float)correctPixels / totalPixels;
    }

    public bool containsGreen()
    {
        byte[] pixels = generatePattern();

        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] == 32) return true;
        }

        return false;
    }

    public static Color[] getColorsFromFile(string file)
    {
        byte[] image = File.ReadAllBytes(file);

        Texture2D tmpTexture = new Texture2D(64, 64);
        tmpTexture.LoadImage(image);
        Color[] pixels = tmpTexture.GetPixels();

        return pixels;
    }

    public bool[] getStencilFromPattern(Pattern pattern)
    {
        if (pattern == Pattern.TopHalf) return topHalfStencil;
        else if (pattern == Pattern.LeftHalf) return leftHalfStencil;
        else if (pattern == Pattern.Sleeves) return sleevesStencil;
        else if (pattern == Pattern.Squiggle) return squiggleStencil;
        else if (pattern == Pattern.Spots) return spotsStencil;
        else if (pattern == Pattern.Bee) return beeStencil;
        else if (pattern == Pattern.Shamrock) return shamrockStencil;
        else if (pattern == Pattern.Unicycle) return unicycleStencil;
        else return PaintingTable.allTrueStencil;
    }

    public byte getColorCodeFromPatternColor(PatternColor patternColor)
    {
        if (patternColor == PatternColor.Green) return 32;
        else if (patternColor == PatternColor.Red) return 33;
        else if (patternColor == PatternColor.Blue) return 34;
        else if (patternColor == PatternColor.Purple) return 35;
        else return 1;
    }
}

public class Imprint
{
    public PatternColor PatternColor;
    public Pattern Pattern;

    public Imprint(PatternColor patternColor, Pattern pattern)
    {
        this.PatternColor = patternColor;
        this.Pattern = pattern;
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
    TopHalf,
    LeftHalf,
    Sleeves,
    Squiggle,
    Spots,
    Bee,
    Shamrock,
    Unicycle
}

public enum PatternColor
{
    Green,
    Red,
    Blue,
    Purple
}