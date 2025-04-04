using UnityEngine;
using System.IO;

public class Order
{
    public ClothingItem ClothingItem;
    public Imprint[] Imprints;
    public PatternColor BaseColor;
    public int SellPrice;

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
        Color[] clothingPixels = PaintingTable.shirtSprite;
        
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

    public bool[] getStencilFromPattern(Pattern pattern)
    {
        if (pattern == Pattern.TopHalf) return PaintingTable.topHalfStencil;
        else if (pattern == Pattern.LeftHalf) return PaintingTable.leftHalfStencil;
        else if (pattern == Pattern.Sleeves) return PaintingTable.sleevesStencil;
        else if (pattern == Pattern.Squiggle) return PaintingTable.squiggleStencil;
        else if (pattern == Pattern.Spots) return PaintingTable.spotsStencil;
        else if (pattern == Pattern.Bee) return PaintingTable.beeStencil;
        else if (pattern == Pattern.Shamrock) return PaintingTable.shamrockStencil;
        else if (pattern == Pattern.Unicycle) return PaintingTable.unicycleStencil;
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