using System.Threading.Tasks;
using Godot;

public class SceneChanger : Control
{
    public static SceneChanger Instance;

    AnimationPlayer player;
    ColorRect colorRect;

    public SceneChanger()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;

        player = GetNode<AnimationPlayer>("CanvasLayer/AnimationPlayer");
        colorRect = GetNode<ColorRect>("CanvasLayer/ColorRect");
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

    async public void GoToScene(string scenePath)
    {
        await FadeOut();

        GetTree().ChangeScene(scenePath);

        await FadeIn();
    }

}