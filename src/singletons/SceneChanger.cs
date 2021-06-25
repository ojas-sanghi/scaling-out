using System.Threading.Tasks;
using Godot;

public class SceneChanger : Control
{
    public static SceneChanger Instance;

    public SceneChanger()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;
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

}