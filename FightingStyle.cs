
using UnityEngine;

public class FightingStyle
{
    public int StyleID { get; set; }
    public string StyleName { get; set; }
    public float Damage { get; set; }
    public float Speed { get; set; }
    public float Range { get; set; }

    // Constructor
    public FightingStyle(int styleID, string styleName, float damage, float speed, float range)
    {
        StyleID = styleID;
        StyleName = styleName;
        Damage = damage;
        Speed = speed;
        Range = range;
    }

    // Method to apply the fighting style to a character
    public void ApplyStyle(Character character)
    {
        character.StyleID = this.StyleID;
    }

    // Method to calculate the damage based on the style
    public float CalculateDamage()
    {
        // The damage calculation can be more complex depending on the game rules
        return this.Damage * this.Speed;
    }

    // Method to calculate the range of the style
    public float CalculateRange()
    {
        // The range calculation can be more complex depending on the game rules
        return this.Range;
    }
}


