using Godot;

[Tool]
public class SelectorSprite : Button
{
    // these are set by DinoSelector parent
    public Texture spriteTexture;
    public string text;
    public Enums.Dinos dinoType;
    public Vector2 customScale = new Vector2(0.511f, 0.519f);

    public bool isAbilitySelector = false;
    public Enums.SpecialAbilities abilityType;
    public Enums.Dinos abilitySelectorAssociatedDino;

    protected Sprite sprite;
    Label label;
    DeployTimer deployTimer;
    Timer cooldownTimer;
    Node2D particles;

    Material blackWhiteShader;

    CombatInfo c = CombatInfo.Instance;
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
        deployTimer.dinoType = dinoType;
        HideParticles();

        // if it's an ability, then disable it at the starting
        if (isAbilitySelector)
        {
            DisableSprite();
            deployTimer.Hide();
        }

        Events.dinoDeployed += OnDinoDeployed;
    }

    public override void _ExitTree()
    {
        Events.dinoDeployed -= OnDinoDeployed;
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
            sprite.Modulate = new Color(1, 1, 1, 0.5f);
            label.Modulate = new Color(1, 1, 1, 0.5f);
        }
    }

    public void UnFadeSprite()
    {
        // only execute if we're a dino
        // AND if we can afford it
        // AND if we're not on cooldown
        if (!isAbilitySelector && d.CanAffordDino(dinoType) && !c.dinosDeploying.Contains(dinoType))
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
        if (isAbilitySelector)
        {
            // only enable self if we're actually deployable
            if (CombatInfo.Instance.IsAbilityDeployable(abilitySelectorAssociatedDino))
                sprite.Material = null;
        }
        else
        {
            // only enable self if we can afford it
            if (d.CanAffordDino(dinoType))
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

    public void CheckCanAfford()
    {
        // only bother if we're a dino selector
        // even if u dont have money left to deploy dinos, u can still use their abilities
        if (isAbilitySelector) return;

        // enable if can afford
        if (d.CanAffordDino(dinoType))
        {
            if (IsInstanceValid(deployTimer)) deployTimer.Show();
            UnFadeSprite();
            EnableSprite();
        }
        else
        {
            cooldownTimer.Stop();
            deployTimer.Hide();
            FadeSprite();
            DisableSprite();
            HideParticles();
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
        c.dinosDeploying.Add(dinoType); // Add to list of dinos just deployed, prevents it from being deployed till cooldown over
        cooldownTimer.Start(d.GetDinoTimerDelay(dinoType));
        await ToSignal(cooldownTimer, "timeout");
        c.dinosDeploying.Remove(dinoType); // remove from list, let it be deployed again
        UnFadeSprite();
    }
}