using System.Threading.Tasks;
using Godot;

public class SceneChanger : Control
{
    async public Task Fade()
    {
        var player = GetNode<AnimationPlayer>("CanvasLayer/AnimationPlayer");

        player.Play("fade");
        await ToSignal(player, "animation_finished");
        GetNode<ColorRect>("CanvasLayer/ColorRect").Hide();
    }

    async public void GoToScene(string scenePath)
    {
        await Fade();

        GetTree().ChangeScene(scenePath);
    }

}