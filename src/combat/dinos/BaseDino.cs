using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public class BaseDino : Area2D
{

    TextureProgress bar;
    AnimationPlayer animPlayer;
    AnimatedSprite animSprite;
    Tween transparencyTween;
    Tween pathTween;
    Timer attackTimer;
    public float pathFollowTime = 1; // set by the "Lane" node when the dino is instanced

    bool dinoDead = false;

    public Enums.Dinos dinoType;
    int dinoVariation; // skin, maybe change this system later

    double spawnDelay;
    bool spawningIn;

    double dinoHealth;
    double animatedHealth;
    public Vector2 dinoSpeed;
    double dinoDmg;
    double dinoDodgeChance;
    double dinoDefense;

    protected List<int> dinoUnlockCost;
    protected Enums.Genes specialGene; // ice, fire, etc

    async public override void _Ready()
    {
        bar = (TextureProgress)FindNode("HealthBar");
        pathTween = (Tween)FindNode("PathFollowTween");
        animPlayer = (AnimationPlayer)FindNode("AnimationPlayer");
        animSprite = (AnimatedSprite)FindNode("AnimatedSprite");
        transparencyTween = (Tween)FindNode("TransparencyTween");
        attackTimer = (Timer)FindNode("AttackingTimer");
        var thumpSound = (AudioStreamPlayer)FindNode("ThumpSound");
        thumpSound.Play();

        await SpawnDelay();

        bar.MaxValue = dinoHealth;
        bar.Value = dinoHealth;
        bar.Show();

        animPlayer.Play(dinoVariation.ToString() + "walk");

        pathTween.InterpolateProperty(
            GetParent(), "unit_offset", 0, 1, pathFollowTime, Tween.TransitionType.Linear, Tween.EaseType.InOut
        );
        pathTween.Start();

        Events.dinoHit += UpdateHealth;
    }

    public override void _ExitTree()
    {
        Events.dinoHit -= UpdateHealth;
    }


    public override void _Process(float delta)
    {
        bar.Value = animatedHealth;
    }

    async Task SpawnDelay()
    {
        bar.Hide();
        animSprite.RotationDegrees = -90;

        GD.Randomize();
        dinoVariation = new RandomNumberGenerator().RandiRange(1, 3);

        animSprite.Animation = dinoVariation.ToString() + "walk";

        transparencyTween.InterpolateProperty(this, "modulate", new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), (float)spawnDelay);
        transparencyTween.Start();
        await ToSignal(transparencyTween, "tween_completed");

        spawningIn = false;

        Events.publishDinoFullySpawned(dinoType);
    }

    protected void CalculateUpgrades()
    {
        UpgradeInfo dinoInfo = DinoInfo.Instance.GetDinoInfo(dinoType);

        dinoHealth = dinoInfo.GetStat(Enums.Stats.Hp);
        animatedHealth = dinoHealth;
        spawnDelay = dinoInfo.GetStat(Enums.Stats.Delay);

        dinoDefense = dinoInfo.GetStat(Enums.Stats.Def);
        dinoDodgeChance = dinoInfo.GetStat(Enums.Stats.Dodge);
        dinoDmg = dinoInfo.GetStat(Enums.Stats.Dmg);
        dinoSpeed = new Vector2((float)dinoInfo.GetStat(Enums.Stats.Speed), 0);
    }

    async Task KillDino()
    {
        var collision = (CollisionPolygon2D)FindNode("CollisionPolygon2D");
        var deathSound = (AudioStreamPlayer)FindNode("DeathSound");

        attackTimer.Stop();
        RemoveFromGroup("dinos");
        Events.publishDinoDiedType(dinoType);
        Events.publishDinoDiedInstance(this);
        collision.SetDeferred("disabled", true);
        pathTween.SetActive(false);

        GD.Randomize();
        var num = GD.Randi() % 2;
        if (num == 0)
        {
            animSprite.FlipH = true;
        }
        else
        {
            animSprite.FlipH = false;
        }

        deathSound.Play();
        animPlayer.Stop();
        animSprite.Play(dinoVariation.ToString() + "death");

        transparencyTween.InterpolateProperty(
            this, "modulate", new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 5
        );
        transparencyTween.Start();
        await ToSignal(transparencyTween, "tween_completed");
        await ToSignal(deathSound, "finished");

        // if there are no dinos, and all the money is spent, then trigger conquest lost signal
        int numDinosLeft = GetTree().GetNodesInGroup("dinos").Count;
        if (CombatInfo.Instance.allMoneyExpended && numDinosLeft == 0)
            Events.publishConquestLost();
        
        QueueFree();
    }

    async public void UpdateHealth(double dmgTaken)
    {
        var healthTween = (Tween)FindNode("HealthTween");

        dmgTaken *= dinoDefense;
        dinoHealth -= dmgTaken;
        healthTween.InterpolateProperty(
            this, "animatedHealth", animatedHealth, dinoHealth, 0.6f, Tween.TransitionType.Linear, Tween.EaseType.In
        );

        if (!healthTween.IsActive())
        {
            healthTween.Start();
        }

        if (dinoHealth <= 0)
        {
            if (!dinoDead)
            {
                dinoDead = true;
                await KillDino();
            }
        }
    }


    void AttackBlockade()
    {
        pathTween.SetActive(false);

        animPlayer.Stop();
        attackTimer.Start();
    }

    void OnAttackingTimerTimeout()
    {
        Events.publishBlockadeHit((float)dinoDmg);
        AttackBlockade();
    }

    void OnBaseDinoAreaEntered(Area2D area)
    {
        if (area.Name.Contains("Blockade"))
        {
            AttackBlockade();
        }
    }
}