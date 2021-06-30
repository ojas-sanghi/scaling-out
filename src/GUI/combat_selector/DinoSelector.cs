using System.Collections.Generic;
using System.Linq;
using Godot;

public class DinoSelector : Node2D
{
    Enums.Dinos selectedDinoType;
    PackedScene selectorScene = GD.Load<PackedScene>("res://src/GUI/combat_selector/SelectorSprite.tscn");
    List<SelectorSprite> selectorList = new List<SelectorSprite>();

    HBoxContainer hBox;

    DinoInfo d = DinoInfo.Instance;

    public override void _Ready()
    {
        Events.dinoDeployed += OnDinoDeployed;
        Events.dinoFullySpawned += ValidateAbilityStatus;
        Events.dinoDiedType += ValidateAbilityStatus;
        Events.selectorSelected += OnSelectorSelected;
        Events.newRound += OnNewRound;

        hBox = (HBoxContainer)FindNode("HBoxContainer");

        SetupSelectors();
        // get a list of children
        selectorList = hBox.GetChildren().Cast<SelectorSprite>().ToList<SelectorSprite>();
        selectorList[0].ShowParticles();
    }

    public override void _ExitTree()
    {
        Events.dinoDeployed += OnDinoDeployed;
        Events.dinoFullySpawned -= ValidateAbilityStatus;
        Events.dinoDiedType -= ValidateAbilityStatus;
        Events.selectorSelected -= OnSelectorSelected;
        Events.newRound -= OnNewRound;
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
            Enums.Dinos associatedDino = d.GetDinoTypeFromAbility(n.Key);

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

            newSelector.text = selectorPositionInList.ToString();

            newSelector.customScale = new Vector2(0.07f, 0.07f);

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
        // if dino
        if (!selector.isAbilitySelector)
        {
            // if (allDinosExpended) return;
            // if can afford dino, turn it on
            if (DinoInfo.Instance.CanAffordDino(selector.dinoType))
            {
                CombatInfo.Instance.selectedDinoType = selector.dinoType;
                EnableExclusiveParticles(selector);
            }

        }
        // if ability
        else
        {
            if (!CombatInfo.Instance.IsAbilityDeployable(selector.abilitySelectorAssociatedDino))
            {
                return;
            }

            // if deployable, and activated
            // shoot projectile from each dino
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
    void ValidateAbilityStatus(Enums.Dinos dinoType)
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

    void ValidateAffordStatus()
    {
        // for each selector, check if they can still afford the dino
        // if not, then they'll fade themselves
        foreach (SelectorSprite ss in selectorList)
            ss.CheckCanAfford();
    }

    void OnDinoDeployed(Enums.Dinos dino)
    {
        ValidateAffordStatus();
    }

    void OnNewRound()
    {
        CombatInfo.Instance.dinosDeploying.Clear();
        ValidateAffordStatus();
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