using UnityEngine;

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