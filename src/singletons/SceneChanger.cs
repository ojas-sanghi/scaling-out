using System;
using System.Threading.Tasks;
using Godot;

public class SceneChanger : Control
{
    public static SceneChanger Instance;

    AnimationPlayer player;
    Label roundLabel;

    public SceneChanger()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;

        player = GetNode<AnimationPlayer>("CanvasLayer/AnimationPlayer");
        roundLabel = GetNode<Label>("CanvasLayer/RoundLabel");
        
        roundLabel.Show();
        roundLabel.SelfModulate = new Color(1, 1, 1, 0);
    }


    // Fades to black
    async public Task FadeOut()
    {
        player.Play("fade_out");
        await ToSignal(player, "animation_finished");
    }

    async public Task FadeIn() 
    {
        player.Play("fade_in");
        await ToSignal(player, "animation_finished");
    }

    public void GoToPackedScene(PackedScene packedScene)
    {
        GoToScene(packedScene.ResourcePath);
    }

    async public void GoToScene(string scenePath)
    {
        await FadeOut();

        GetTree().ChangeScene(scenePath);

        await FadeIn();
    }

    /////////////////

    // "continue conquest" pressed --> this called --> publishNewRound() triggers everything else
    async public void NewRoundAnimation()
    {
        float animDuration = 2f;

        await FadeOut();

        roundLabel.Text = "ROUND " + CombatInfo.Instance.currentRound.ToString();
        
        Tween tween = new Tween();
        AddChild(tween);

        // fade in the label
        tween.InterpolateProperty(roundLabel, "self_modulate", Color.FromHsv(0, 1, 1, 0), Color.FromHsv(0, 1, 1, 1), animDuration);
        tween.Start();
        await ToSignal(tween, "tween_all_completed");
        
        // color change 10 different stuffs
        for (int i = 0; i < 11; i++)
        {
            tween.InterpolateProperty(roundLabel, "self_modulate", null, Color.FromHsv(0.1f * i, 1, 1, 1), animDuration / 10);
            tween.Start();
            await ToSignal(tween, "tween_all_completed");
        }

        // current color
        var c = roundLabel.SelfModulate;
        // construct a color that is current color but transparent
        Color transparentColor = new Color(c.r, c.g, c.g, 0);
        // fade out the label
        tween.InterpolateProperty(roundLabel, "self_modulate", null, transparentColor, animDuration);
        tween.Start();  
        await ToSignal(tween, "tween_all_completed");

        Events.publishNewRound();

        await FadeIn();

    }
}