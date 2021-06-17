using System.Collections.Generic;
using System.Linq;
using Godot;

public class DinoSelector : Node2D
{
    Enums.Dinos activeId;
    PackedScene selectorScene = GD.Load<PackedScene>("res://src/GUI/combat_selector/SelectorSprite.tscn");
    List<SelectorSprite> selectorList = new List<SelectorSprite>();

    HBoxContainer hBox;

    DinoInfo d = DinoInfo.Instance;

    bool allDinosExpended = false;

    public override void _Ready()
    {
        Events.dinoFullySpawned += OnDinoFullySpawned;
        Events.dinoDied += OnDinoDied;
        Events.allDinosExpended += OnAllDinosExpended;
        Events.dinosPurchased += OnDinosPurchased;

        hBox = (HBoxContainer)FindNode("HBoxContainer");

        SetupSelectors();
        // get a list of children
        selectorList = hBox.GetChildren().Cast<SelectorSprite>().ToList<SelectorSprite>();
        selectorList[0].ShowParticles();

    }

    public override void _ExitTree()
    {
        Events.dinoFullySpawned -= OnDinoFullySpawned;
        Events.dinoDied -= OnDinoDied;
        Events.allDinosExpended -= OnAllDinosExpended;
        Events.dinosPurchased -= OnDinosPurchased;
    }

    void SetupSelectors()
    {
        int iconId = 0;
        foreach (KeyValuePair<Enums.Dinos, StreamTexture> n in d.dinoIcons)
        {
            SelectorSprite newSelector = (SelectorSprite)selectorScene.Instance();
            newSelector.spriteTexture = n.Value;
            newSelector.dinoId = n.Key;
            newSelector.text = (iconId + 1).ToString();

            hBox.AddChild(newSelector);

            iconId++;
        }

        int abilityId = 0;
        foreach (KeyValuePair<Enums.Dinos, StreamTexture> n in d.dinoAbilityIcons)
        {
            // skip if no icon
            if (n.Value == null)
            {
                continue;
            }

            // TODO: FOR THE LOVE OF GOD PLEASE CHANGE THIS LOGIC
            // find the filename of the image, which is also the name of the ability itself
            var fileName = n.Value.ResourcePath;
            var abilityStart = fileName.FindLast("/");
            var abilityEnd = fileName.Find(".png");
            var abilityString = fileName.Substr(abilityStart, abilityEnd);

            SelectorSprite newSelector = (SelectorSprite)selectorScene.Instance();
            newSelector.spriteTexture = n.Value;
            newSelector.dinoId = n.Key;
            newSelector.text = (abilityId + d.dinoIcons.Count + 1).ToString(); // position in list + number of dinos

            newSelector.abilityMode = abilityString;
            newSelector.customScale = new Vector2((float)0.07, (float)0.07);

            hBox.AddChild(newSelector);

            abilityId++;
        }
    }

    // turn on particles for this selector and turns off all other particles
    void EnableExclusiveParticles(int index)
    {
        var selectors = hBox.GetChildren().Cast<SelectorSprite>().ToList<SelectorSprite>();
        foreach (SelectorSprite s in selectors)
        {
            s.HideParticles();
        }
        selectors[index].ShowParticles();

    }

    public override void _Input(InputEvent @event)
    {
        if (allDinosExpended)
        {
            return;
        }

        // TODO: change this. #107, https://app.gitkraken.com/glo/view/card/a9b9034aa0834eb5bdabe2aac01a4200
        if (@event.IsActionPressed("dino_1"))
        {
            activeId = Enums.Dinos.Mega;
            EnableExclusiveParticles(0);
        }
        else if (@event.IsActionPressed("dino_2"))
        {
            activeId = Enums.Dinos.Tanky;
            EnableExclusiveParticles(1);
        }
        else if (@event.IsActionPressed("dino_3"))
        {
            activeId = Enums.Dinos.Warrior;
            EnableExclusiveParticles(2);
        }
        else if (@event.IsActionPressed("dino_4"))
        {
            activeId = Enums.Dinos.Gator;
            EnableExclusiveParticles(3);
        }

        // TODO: change this. #73, https://app.gitkraken.com/glo/view/card/75f5162699514eddb32954a7629c6423
        else if (@event.IsActionPressed("dino_5"))
        {
            if (!(d.GetDinoInfo(Enums.Dinos.Tanky).UnlockedSpecial()) || CombatInfo.Instance.shotIce)
            {
                return;
            }

            foreach (BaseDino d in GetTree().GetNodesInGroup("dinos"))
            {
                if (d.dinoType == Enums.Dinos.Tanky)
                {
                    TankyDino tanky = (TankyDino)d;
                    tanky.ShootProjectile();
                    selectorList[4].DisableSprite();
                    CombatInfo.Instance.shotIce = true;
                    return;
                }
            }

        }
        else if (@event.IsActionPressed("dino_6"))
        {
            if (!(d.GetDinoInfo(Enums.Dinos.Warrior).UnlockedSpecial()) || CombatInfo.Instance.shotFire)
            {
                return;
            }

            foreach (BaseDino d in GetTree().GetNodesInGroup("dinos"))
            {
                if (d.dinoType == Enums.Dinos.Warrior)
                {
                    WarriorDino warrior = (WarriorDino)d;
                    warrior.ShootProjectile();
                    selectorList[5].DisableSprite();
                    CombatInfo.Instance.shotFire = true;
                    return;
                }
            }

        }

        CombatInfo.Instance.dinoId = activeId;
    }

    void OnDinoFullySpawned()
    {
        Enums.Dinos dinoType = (Enums.Dinos)d.GetDinoProperty("dinoType");

        // TODO: do this better
        // TODO: fix this lol

        if (dinoType == Enums.Dinos.Tanky)
        {
            if (d.GetDinoInfo(dinoType).UnlockedSpecial() && !(CombatInfo.Instance.shotIce))
            {
                selectorList[4].EnableSprite();
            }
        }

        if (dinoType == Enums.Dinos.Warrior)
        {
            if (d.GetDinoInfo(dinoType).UnlockedSpecial() && !CombatInfo.Instance.shotFire)
            {
                selectorList[5].EnableSprite();
            }
        }

    }

    void OnDinoDied(Enums.Dinos type)
    {
        var dinosLeft = GetTree().GetNodesInGroup("dinos");

        // TODO: do this better

        if (type == Enums.Dinos.Tanky)
        {
            foreach (BaseDino d in dinosLeft)
            {
                if (d.dinoType == Enums.Dinos.Tanky)
                {
                    return;
                }
            }
            selectorList[4].DisableSprite();
        }

        if (type == Enums.Dinos.Warrior)
        {
            foreach (BaseDino d in dinosLeft)
            {
                if (d.dinoType == Enums.Dinos.Warrior)
                {
                    return;
                }
            }
            selectorList[5].DisableSprite();
        }
    }

    void OnAllDinosExpended()
    {
        allDinosExpended = true;
    }

    async void OnDinosPurchased(int numDinos)
    {
        // Wait for 0.1 seconds to allow for the same signal to be registered and executed in Combat.cs
        // Only once that code executes will our un-fading code work properly
        // Kinda hacky but oh well
        await ToSignal(GetTree().CreateTimer((float)0.1), "timeout");
        
        // reset switch, unfade all sprites once more dinos are bought
        if (CombatInfo.Instance.dinosRemaining > 0)
        {
            allDinosExpended = false;
            foreach (SelectorSprite ss in selectorList)
            {
                ss.EnableSprite();
            }
        }
    }

}