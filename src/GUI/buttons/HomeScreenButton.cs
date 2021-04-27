using Godot;

[Tool]
public class HomeScreenButton : TextureButton
{
    [Export] Enums.HomeScreenButtons mode = Enums.HomeScreenButtons.None;

    Label label;

    public override void _Ready()
    {
        label = (Label)FindNode("Label");
        if (Engine.EditorHint)
        {
            return;
        }

        if (mode == Enums.HomeScreenButtons.None)
        {
            GD.PushError("HomeScreenButton mode must be set");
            GD.PrintStack();
            GetTree().Quit(1);
        }
        SetButtonText();
    }

    public override void _Process(float delta)
    {
        if (Engine.EditorHint)
        {
            if (label.Text == "example text")
            {
                SetButtonText();
            }
        }
    }


    void SetButtonText()
    {
        switch (mode)
        {
            case Enums.HomeScreenButtons.Map:
                label.Text = "Go fight!";
                break;
            case Enums.HomeScreenButtons.Upgrades:
                label.Text = "View upgrade menu";
                break;
        }
    }

    void OnButtonPressed()
    {
        switch (mode)
        {
            case Enums.HomeScreenButtons.Map:
                SceneChanger.Instance.GoToScene("res://src/combat/CombatScreen.tscn");
                break;
            case Enums.HomeScreenButtons.Upgrades:
                SceneChanger.Instance.GoToScene("res://src/GUI/screens/UpgradeScreen.tscn");
                break;
        }
    }

}