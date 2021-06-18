using System;
using System.Collections.Generic;

public class PlayerStats 
{
    public static int gold = 30000;
    public static int genes = 30000;

    public static List<Enums.Dinos> dinosUnlocked = new List<Enums.Dinos>() { Enums.Dinos.Mega, Enums.Dinos.Tanky, Enums.Dinos.Warrior };
    public static List<Enums.Dinos> dinsoWithSpecialsUnlocked = new List<Enums.Dinos>() { Enums.Dinos.Mega, Enums.Dinos.Tanky };
    public static List<Enums.Genes> genesFound = new List<Enums.Genes>();
}