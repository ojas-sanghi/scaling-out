using Godot;

[Tool]
public class SelectorSprite : Control
{
    // these are set by DinoSelector parent
    public Texture spriteTexture;
    public string text;
    public Enums.Dinos dinoId;

    public string abilityMode = ""; //TODO: change this once the file logic in DinoSelector is changed
    public Vector2 customScale = new Vector2((float)0.511, (float)0.519);

    Sprite sprite;
    Label label;
    Sprite disabled;
    DeployTimer deployTimer;
    Node2D particles;

    public override void _Ready()
    {
        sprite = (Sprite)FindNode("Sprite");
        label = (Label)FindNode("Label");
        disabled = (Sprite)FindNode("disabled");
        deployTimer = (DeployTimer)FindNode("DeployTimer");
        particles = (Node2D)FindNode("Particles");

        sprite.Texture = spriteTexture;
        sprite.Scale = customScale;
        label.Text = text;
        HideParticles();

        if (abilityMode == "")
        {
            disabled.Hide();
        }
        else
        {
            DisableAbility();
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


    void FadeSprite()
    {
        sprite.Modulate = new Color(1, 1, 1, (float)0.5);
        label.Modulate = new Color(1, 1, 1, (float)0.5);
    }

    void UnFadeSprite()
    {
        // only unfade if we have more dinos left
        if (CombatInfo.Instance.dinosRemaining != 0)
        {
            sprite.Modulate = new Color(1, 1, 1, 1);
            label.Modulate = new Color(1, 1, 1, 1);
        }
    }

    public void DisableAbility()
    {
        if (abilityMode == "none")
        {
            return;
        }
        disabled.Show();
        FadeSprite();
    }

    public void EnableAbility()
    {
        if (abilityMode == "none")
        {
            return;
        }
        disabled.Hide();
        UnFadeSprite();
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

    async void OnDinoDeployed()
    {
        if (CombatInfo.Instance.dinoId != dinoId)
        {
            return;
        }

        FadeSprite();
        await ToSignal(
            GetTree().CreateTimer((float)DinoInfo.Instance.GetDinoTimerDelay(), false),
            "timeout"
        );
        UnFadeSprite();
    }

    void OnAllDinosExpended()
    {
        // don't fade the sprite if it's an ability
        // even if there aren't any dinos left to deploy you can still use abilities
        if (abilityMode == "none")
        {
            FadeSprite();
            HideParticles();
            deployTimer.Hide();
        }
    }


}