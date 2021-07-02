 using System.Threading.Tasks;
using Godot;

public class SceneChanger : Control
{
    public static SceneChanger Instance;

    Tween tween;

    public SceneChanger()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;

        tween = GetNode<Tween>("Tween");
    }

    async public Task Fade()
    {
        var player = GetNode<AnimationPlayer>("CanvasLayer/AnimationPlayer");
        var colorRect = GetNode<ColorRect>("CanvasLayer/ColorRect");

        player.Play("fade");
        await ToSignal(player, "animation_finished");
        colorRect.Hide();
    }

    async public void GoToScene(string scenePath)
    {
        await Fade();

        GetTree().ChangeScene(scenePath);
    }

    // slide in next combat screen from the right side
    async public Task NextCombatRound()
    {
        CombatScreen currentCombat = GetNode<CombatScreen>("/root/CombatScreen");
        CombatScreen newCombat = (CombatScreen)GD.Load<PackedScene>("res://src/combat/CombatScreen.tscn").Instance();
        newCombat.RectPosition = new Vector2(1920, 0);
        GetNode("/root").AddChild(newCombat);

        // move current screen from (0, 0) to (-1920, 0) (off-screen)
        // move new screen from (1920, 0) (off-screen) to (0, 0)
        int tweenDuration = 2;
        tween.InterpolateProperty(currentCombat, "rect_position", null, new Vector2(-1920, 0), tweenDuration);
        tween.InterpolateProperty(newCombat, "rect_position", null, Vector2.Zero, tweenDuration);
        tween.Start();
        await ToSignal(tween, "tween_all_completed");
        currentCombat.QueueFree();
        CombatInfo.combatScreenName = newCombat.Name;
    }

    // TODO
    // - dino counter label not working
    // - crashes when i deploy dino
    // - army are just randomly shooting

}