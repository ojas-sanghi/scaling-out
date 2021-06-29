using System;
using System.Collections.Generic;
using Godot;

public class DinoInfo : Node
{
    public static DinoInfo Instance;

    public int dinoCredCost = 30;

    public Dictionary<Enums.Dinos, PackedScene> dinoList;
    public Dictionary<Enums.Dinos, StreamTexture> dinoIcons;
    public Dictionary<Enums.Dinos, UpgradeInfo> upgradesInfo;
    public Dictionary<Enums.Dinos, Enums.SpecialAbilities> dinoTypesAndAbilities;
    public Dictionary<Enums.SpecialAbilities, StreamTexture> specialAbilityIcons;
    public Dictionary<Enums.SpecialAbilities, VideoStream> specialAbilityVidPreviews;

    public DinoInfo()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;

        dinoList = new Dictionary<Enums.Dinos, PackedScene>()
        {
            {Enums.Dinos.Mega, GD.Load<PackedScene>("res://src/actors/dinos/MegaDino.tscn")},
            {Enums.Dinos.Tanky, GD.Load<PackedScene>("res://src/actors/dinos/TankyDino.tscn")},
            {Enums.Dinos.Warrior, GD.Load<PackedScene>("res://src/actors/dinos/WarriorDino.tscn")},
            {Enums.Dinos.Gator, GD.Load<PackedScene>("res://src/actors/dinos/GatorGecko.tscn")},
        };

        dinoIcons = new Dictionary<Enums.Dinos, StreamTexture>()
        {
            {Enums.Dinos.Mega, GD.Load<StreamTexture>("res://assets/dinos/mega_dino/mega_dino.png")},
            {Enums.Dinos.Tanky, GD.Load<StreamTexture>("res://assets/dinos/tanky_dino/Armored_Dino_ICON.png")},
            {Enums.Dinos.Warrior, GD.Load<StreamTexture>("res://assets/dinos/warrior_dino/Tribal_Dino_icon.png")},
            {Enums.Dinos.Gator, GD.Load<StreamTexture>("res://assets/dinos/gator_gecko/gator_gecko_icon.png")},
        };

        upgradesInfo = new Dictionary<Enums.Dinos, UpgradeInfo>()
        {
            {Enums.Dinos.Mega, new UpgradeInfo("res://src/actors/dinos/stats/MegaDino.tres")},
            {Enums.Dinos.Tanky, new UpgradeInfo("res://src/actors/dinos/stats/TankyDino.tres")},
            {Enums.Dinos.Warrior, new UpgradeInfo("res://src/actors/dinos/stats/WarriorDino.tres")},
            {Enums.Dinos.Gator, new UpgradeInfo("res://src/actors/dinos/stats/GatorGecko.tres")}
        };

        dinoTypesAndAbilities = new Dictionary<Enums.Dinos, Enums.SpecialAbilities>()
        {
            {Enums.Dinos.Mega, Enums.SpecialAbilities.None},
            {Enums.Dinos.Tanky, Enums.SpecialAbilities.IceProjectile},
            {Enums.Dinos.Warrior, Enums.SpecialAbilities.FireProjectile},
            {Enums.Dinos.Gator, Enums.SpecialAbilities.None},
        };

        specialAbilityIcons = new Dictionary<Enums.SpecialAbilities, StreamTexture>()
        {
            {Enums.SpecialAbilities.IceProjectile, GD.Load<StreamTexture>("res://assets/dinos/misc/ice.png")},
            {Enums.SpecialAbilities.FireProjectile, GD.Load<StreamTexture>("res://assets/dinos/misc/fire.png")},
        };

        specialAbilityVidPreviews = new Dictionary<Enums.SpecialAbilities, VideoStream>()
        {
            {Enums.SpecialAbilities.IceProjectile, GD.Load<VideoStream>("res://assets/abilities/previews/ice-preview.ogv")},
            {Enums.SpecialAbilities.FireProjectile, new VideoStreamWebm()},
        };

    }

    public UpgradeInfo GetDinoInfo(Enums.Dinos dino)
    {
        return upgradesInfo[dino];
    }

    public float GetDinoTimerDelay(Enums.Dinos dinoType)
    {
        return (float)GetDinoProperty(dinoType, "spawnDelay");
    }

    // Instance dino, get variable we want, then remove it
    public object GetDinoProperty(Enums.Dinos dinoType, string property)
    {
        PackedScene DinoScene = dinoList[dinoType];
        BaseDino DinoInstance = (BaseDino)DinoScene.Instance();

        object DinoProperty = DinoInstance.Get(property);

        DinoInstance.QueueFree();
        return DinoProperty;
    }

}