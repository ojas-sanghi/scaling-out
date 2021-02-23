using System.Collections.Generic;
using Godot;

public class DinoInfo : Node
{
    public static DinoInfo Instance;

    public Dictionary<Enums.Dinos, PackedScene> dinoList = new Dictionary<Enums.Dinos, PackedScene>() 
    {
        {Enums.Dinos.Mega, GD.Load<PackedScene>("res://src/actors/dinos/MegaDino.tscn")},
        {Enums.Dinos.Tanky, GD.Load<PackedScene>("res://src/actors/dinos/TankyDino.tscn")},
        {Enums.Dinos.Warrior, GD.Load<PackedScene>("res://src/actors/dinos/WarriorDino.tscn")},
        {Enums.Dinos.Gator, GD.Load<PackedScene>("res://src/actors/dinos/GatorGecko.tscn")},
    };

    public Dictionary<Enums.Dinos, StreamTexture> dinoIcons = new Dictionary<Enums.Dinos, StreamTexture>() 
    {
        {Enums.Dinos.Mega, GD.Load<StreamTexture>("res://assets/dinos/mega_dino/mega_dino.png")},
        {Enums.Dinos.Tanky, GD.Load<StreamTexture>("res://assets/dinos/tanky_dino/Armored_Dino_ICON.png")},
        {Enums.Dinos.Warrior, GD.Load<StreamTexture>("res://assets/dinos/warrior_dino/Tribal_Dino_icon.png")},
        {Enums.Dinos.Gator, GD.Load<StreamTexture>("res://assets/dinos/gator_gecko/gater_gecko_icon.png")},
    };

    public Dictionary<Enums.Dinos, StreamTexture> dinoAbilityIcons = new Dictionary<Enums.Dinos, StreamTexture>() 
    {
        {Enums.Dinos.Mega, new StreamTexture()},
        {Enums.Dinos.Tanky, GD.Load<StreamTexture>("res://assets/dinos/tanky_dino/Armored_Dino_ICON.png")},
        {Enums.Dinos.Warrior, GD.Load<StreamTexture>("res://assets/dinos/warrior_dino/Tribal_Dino_icon.png")},
        {Enums.Dinos.Gator, new StreamTexture()},
    };

    public Dictionary<Enums.Dinos, UpgradeInfo> upgradesInfo = new Dictionary<Enums.Dinos, UpgradeInfo>()
    {
        {Enums.Dinos.Mega, new UpgradeInfo("res://src/actors/dinos/stats/MegaDino.tres")},
        {Enums.Dinos.Tanky, new UpgradeInfo("res://src/actors/dinos/stats/TankyDino.tres")},
        {Enums.Dinos.Warrior, new UpgradeInfo("res://src/actors/dinos/stats/WarriorDino.tres")},
        {Enums.Dinos.Gator, new UpgradeInfo("res://src/actors/dinos/stats/GatorDino.tres")}
    };

    public override void _Ready()
    {
        Instance = this;        
    }
    
    public UpgradeInfo GetDino(Enums.Dinos dino) {
        return upgradesInfo[dino];
    }

    public double GetDinoTimerDelay() {
        return (double) GetDinoProperty("spawnDelayValue");
    }

    // Instance dino, get variable we want, then remove it
    public object GetDinoProperty(string property) {
        PackedScene DinoScene = dinoList[CombatInfo.Instance.dinoId];
        Node DinoInstance = DinoScene.Instance();

        object DinoProperty = DinoInstance.Get(property);

        DinoInstance.QueueFree();
        return DinoProperty;
    }

}