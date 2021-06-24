using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public class DinoSelector : Node2D
{
    Enums.Dinos selectedDinoType;
    PackedScene selectorScene = GD.Load<PackedScene>("res://src/GUI/combat_selector/SelectorSprite.tscn");
    List<SelectorSprite> selectorList = new List<SelectorSprite>();

    HBoxContainer hBox;

    DinoInfo d = DinoInfo.Instance;

    bool allDinosExpended = false;

    public override void _Ready()
    {
        Events.dinoFullySpawned += validateAbilityStatus;
        Events.dinoDied += validateAbilityStatus;
        Events.allDinosExpended += OnAllDinosExpended;
        Events.dinosPurchased += OnDinosPurchased;
        Events.selectorSelected += OnSelectorSelected;

        hBox = (HBoxContainer)FindNode("HBoxContainer");

        SetupSelectors();
        // get a list of children
        selectorList = hBox.GetChildren().Cast<SelectorSprite>().ToList<SelectorSprite>();
        selectorList[0].ShowParticles();
    }

    public override void _ExitTree()
    {
        Events.dinoFullySpawned -= validateAbilityStatus;
        Events.dinoDied -= validateAbilityStatus;
        Events.allDinosExpended -= OnAllDinosExpended;
        Events.dinosPurchased -= OnDinosPurchased;
    }

    void SetupSelectors()
    {
        int selectorPositionInList = 1;
        foreach (KeyValuePair<Enums.Dinos, StreamTexture> n in d.dinoIcons)
        {
            // skip if not unlocked the dino yet
            if (!PlayerStats.Instance.dinosUnlocked.Contains(n.Key))
            {
                continue;
            }

            SelectorSprite newSelector = (SelectorSprite)selectorScene.Instance();
            newSelector.spriteTexture = n.Value;
            newSelector.dinoType = n.Key;
            newSelector.text = selectorPositionInList.ToString();

            newSelector.Shortcut = new ShortCut();
            newSelector.Shortcut.Shortcut = new InputEventKey();
            ((InputEventKey)newSelector.Shortcut.Shortcut).Scancode = ((int)Godot.KeyList.Key0) + (uint)selectorPositionInList;

            hBox.AddChild(newSelector);

            selectorPositionInList++;
        }

        foreach (KeyValuePair<Enums.SpecialAbilities, StreamTexture> n in d.specialAbilityIcons)
        {
            // lookup dictionary to get key by value
            // ty :) https://stackoverflow.com/questions/2444033/get-dictionary-key-by-value#2444064
            Enums.Dinos associatedDino = d.dinoTypesAndAbilities.FirstOrDefault(x => x.Value == n.Key).Key;

            // skip if not unlocked the speical for the dino yet or if not unlocked the dino itself
            if (!PlayerStats.Instance.dinosUnlocked.Contains(associatedDino) || !d.GetDinoInfo(associatedDino).HasSpecial())
            {
                continue;
            }

            SelectorSprite newSelector = (SelectorSprite)selectorScene.Instance();
            newSelector.isAbilitySelector = true;
            newSelector.spriteTexture = n.Value;
            newSelector.abilityType = n.Key;
            newSelector.dinoType = Enums.Dinos.None;
            newSelector.abilitySelectorAssociatedDino = associatedDino;

            newSelector.text = selectorPositionInList.ToString(); // position in list + number of dinos

            newSelector.customScale = new Vector2((float)0.07, (float)0.07);

            newSelector.Shortcut = new ShortCut();
            newSelector.Shortcut.Shortcut = new InputEventKey();
            ((InputEventKey)newSelector.Shortcut.Shortcut).Scancode = ((int)Godot.KeyList.Key0) + (uint)selectorPositionInList;

            hBox.AddChild(newSelector);

            selectorPositionInList++;
        }
    }

    // turn on particles for this selector and turns off all other particles
    void EnableExclusiveParticles(SelectorSprite selectorToKeepOn)
    {
        var selectors = hBox.GetChildren().Cast<SelectorSprite>().ToList<SelectorSprite>();
        foreach (SelectorSprite s in selectors)
        {
            s.HideParticles();
            if (s == selectorToKeepOn) s.ShowParticles();
        }
    }

    void OnSelectorSelected(SelectorSprite selector)
    {
        if (!selector.isAbilitySelector)
        {
            if (allDinosExpended) return;

            CombatInfo.Instance.selectedDinoType = selector.dinoType;
            EnableExclusiveParticles(selector);
        }
        else
        {
            if (!CombatInfo.Instance.IsAbilityDeployable(selector.abilitySelectorAssociatedDino))
            {
                return;
            }

            foreach (BaseDino d in GetTree().GetNodesInGroup("dinos"))
            {
                if (d.dinoType == selector.abilitySelectorAssociatedDino)
                {
                    var abilityDino = (AbilityDino)d;
                    abilityDino.ShootProjectile();
                    GetAbilitySelector(selector.abilityType).DisableSprite();

                    CombatInfo.Instance.abilitiesUsed.Append(DinoInfo.Instance.dinoTypesAndAbilities[d.dinoType]);
                }
            }
        }


    }

    // when dinos are spawned/die, check if any associated special abilities should be enabled/disabled
    void validateAbilityStatus(Enums.Dinos dinoType)
    {
        // get ability selector for associated dino type
        // then turn it on/off according 
        var selector = GetAbilitySelectorOrNull(d.dinoTypesAndAbilities[dinoType]);
        if (selector == null) return;

        if (CombatInfo.Instance.IsAbilityDeployable(dinoType))
        {
            selector.EnableSprite();
        }
        else
        {
            selector.DisableSprite();
        }

    }

    async void OnDinosPurchased(int numDinos)
    {
        // Wait for 0.1 seconds to allow for the same signal to be registered and executed in Combat.cs
        // Only once that code executes will our un-fading code work properly
        // Kinda hacky but oh well
        await ToSignal(GetTree().CreateTimer((float)0.1), "timeout");

        // reset switch, re-enable all sprites once more dinos are bought
        if (CombatInfo.Instance.dinosRemaining > 0)
        {
            allDinosExpended = false;
            foreach (SelectorSprite ss in selectorList)
            {
                ss.EnableSprite();
            }
        }
    }

    void OnAllDinosExpended()
    {
        allDinosExpended = true;
    }

    SelectorSprite GetDinoSelectorOrNull(Enums.Dinos dinoType)
    {
        foreach (SelectorSprite s in selectorList)
        {
            if (!s.isAbilitySelector && s.dinoType == dinoType)
            {
                return s;
            }
        }
        return null;
    }

    SelectorSprite GetDinoSelector(Enums.Dinos dinoType)
    {
        var dinoSelector = GetDinoSelectorOrNull(dinoType);
        if (dinoSelector == null)
        {
            throw new KeyNotFoundException("Couldn't find dino selector for passed in parameter: " + dinoType);
        }
        else
        {
            return dinoSelector;
        }
    }

    SelectorSprite GetAbilitySelectorOrNull(Enums.SpecialAbilities ability)
    {
        foreach (SelectorSprite s in selectorList)
        {
            if (s.isAbilitySelector && s.abilityType == ability)
            {
                return s;
            }
        }
        return null;
    }

    SelectorSprite GetAbilitySelector(Enums.SpecialAbilities ability)
    {
        var abilitySelector = GetAbilitySelectorOrNull(ability);
        if (abilitySelector == null)
        {
            throw new KeyNotFoundException("Couldn't find ability selector for passed in parameter: " + ability);
        }
        else
        {
            return abilitySelector;
        }
    }
}