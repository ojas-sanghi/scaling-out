using Godot;

[Tool]
public class SelectorSprite : Button
{
    // these are set by DinoSelector parent
    public Texture spriteTexture;
    public string text;
    public Enums.Dinos dinoType;
    public Vector2 customScale = new Vector2((float)0.511, (float)0.519);

    public bool isAbilitySelector = false;
    public Enums.SpecialAbilities abilityType;
    public Enums.Dinos abilitySelectorAssociatedDino;

    protected Sprite sprite;
    Label label;
    DeployTimer deployTimer;
    Timer cooldownTimer;
    Node2D particles;

    Material blackWhiteShader;

    DinoInfo d = DinoInfo.Instance;

    public override void _Ready()
    {
        if (Engine.EditorHint)
        {
            return;
        }

        sprite = (Sprite)FindNode("Sprite");
        label = (Label)FindNode("Label");
        deployTimer = (DeployTimer)FindNode("DeployTimer");
        cooldownTimer = (Timer)FindNode("CooldownTimer");
        particles = (Node2D)FindNode("Particles");
        blackWhiteShader = GD.Load<Material>("res://assets/shaders/BlackWhiteShaderMaterial.tres");

        sprite.Texture = spriteTexture;
        sprite.Scale = customScale;
        label.Text = text;
        HideParticles();

        // if it's an ability, then disable it at the starting
        if (isAbilitySelector)
        {
            DisableSprite();
            deployTimer.Hide();
        }

        Events.dinoDeployed += OnDinoDeployed;
        Events.allDinosExpended += OnAllDinosExpended;
    }

    public override void _ExitTree()
    {
        Events.dinoDeployed -= OnDinoDeployed;
        Events.allDinosExpended -= OnAllDinosExpended;
    }

    void OnSelectorSpritePressed()
    {
        Events.publishSelectorSelected(this);
    }

    void FadeSprite()
    {
        // only execute if we're a dino
        if (!isAbilitySelector)
        {
            sprite.Modulate = new Color(1, 1, 1, (float)0.5);
            label.Modulate = new Color(1, 1, 1, (float)0.5);
        }
    }

    public void UnFadeSprite()
    {
        // only execute if we're a dino
        if (!isAbilitySelector)
        {
            sprite.Modulate = new Color(1, 1, 1, 1);
            label.Modulate = new Color(1, 1, 1, 1);
        }

    }

    // this just makes the sprite black and white
    public void DisableSprite()
    {
        sprite.Material = blackWhiteShader;
    }

    // this just makes the sprite back to normal color, no longer black and white
    public void EnableSprite()
    {
        // if we're an ability selector, cast ourselves to that specific class
        // then call that specific EnableSprite()
        if (isAbilitySelector)
        {
            // only enable self if we're actually deployable
            if (CombatInfo.Instance.IsAbilityDeployable(abilitySelectorAssociatedDino))
            {
                sprite.Material = null;
            }
        }
        else
        {
            // if we're a dino -- then enable it regardless
            sprite.Material = null;
        }
    }

    public void HideParticles()
    {
        foreach (Particles2D particle in particles.GetChildren())
        {
            particle.Emitting = false;
        }
    }

    public void ShowParticles()
    {
        foreach (Particles2D particle in particles.GetChildren())
        {
            particle.Emitting = true;
        }
    }

    async void OnDinoDeployed(Enums.Dinos _dinoType)
    {
        // only bother if the dino being deployed is our associated ID
        // and if we're a dino selector
        if (_dinoType != this.dinoType || isAbilitySelector)
        {
            return;
        }

        FadeSprite();
        cooldownTimer.Start((float)DinoInfo.Instance.GetDinoTimerDelay(dinoType));
        await ToSignal(cooldownTimer, "timeout");
        UnFadeSprite();
    }

    void OnAllDinosExpended()
    {
        // don't fade the sprite if it's an ability
        // even if there aren't any dinos left to deploy you can still use abilities
        if (isAbilitySelector)
        {
            return;
        }
        // if the dino is in the middle of a cooldown after being deployed, then stop it
        cooldownTimer.Stop();

        // unfade them so that they don't look weird when made B&W
        UnFadeSprite();

        DisableSprite();
        HideParticles();
        deployTimer.Hide();
    }
}