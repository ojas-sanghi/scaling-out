using Enums;
using System;
using System.ComponentModel;

public static class EnumUtils
{
    public static string GetUSAPlaceName<T>(this T place) where T : struct
    {
        // ensure they pass an enum
        Type type = place.GetType();
        if (!type.IsEnum)
        {
            throw new ArgumentException($"{nameof(place)} must be of Enum type", nameof(place));
        }

        // return enum's description if it has it; if not, return the enum as a string
        var memberInfo = type.GetMember(place.ToString());
        if (memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }
        return place.ToString();
    }

    public static string GetSpecialAbilityName(SpecialAbilities value) => value switch
    {
        SpecialAbilities.IceProjectile => "Ice Projectile",
        SpecialAbilities.FireProjectile => "Fire Projectile",
        _ => value.ToString(),
    };

    public static string GetSpecialAbilityDescription(SpecialAbilities value) => value switch
    {
        SpecialAbilities.IceProjectile => "Ice projectile that temporarily slows down the rate of fire of army soldiers with guns",
        SpecialAbilities.FireProjectile => "Fire projectile that temporarily jams the guns of army soldiers, preventing them from shooting",
        _ => throw new ArgumentException("The SpecialAbilities value passed in was not valid. Parameter: " + value),
    };

    public static string GetDinoName(Enums.Dinos value) => value switch
    {
        Dinos.Mega => "Mega Dino",
        Dinos.Tanky => "Tanky Dino",
        Dinos.Warrior => "Warrior Dino",
        Dinos.Gator => "Gator Gecko",
        _ => throw new ArgumentException("The Enums.Dinos value passed in was not valid. Parameter: " + value),

    };

}