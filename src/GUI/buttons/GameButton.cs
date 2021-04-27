using Godot;
using b = Enums.GameButtonModes;

[Tool]
public class GameButton : Button
{
    [Export] public b buttonMode;

    public override void _Ready()
    {
        if (Engine.EditorHint)
        {
            return;
        }

        if (buttonMode == b.None)
        {
            GD.PushError("You must set buttonMode for GameButton! Located at: " + GetTree().CurrentScene.Filename);
            GD.PrintStack();
            GetTree().Quit(1);
        }
    }

    public override void _Process(float delta)
    {
        if (Engine.EditorHint)
        {
            if (Text == "example text")
            {
                SetButtonText();
            }
        }
    }

    void SetButtonText()
    {
        switch (buttonMode)
        {
            case b.Quit:
                Text = "Quit";
                break;
            case b.Play:
                Text = "Play";
                this.GrabFocus();
                break;
            case b.RetryCombat:
                Text = "Retry";
                this.GrabFocus();
                break;
            case b.RetryStealth:
                Text = "Retry";
                this.GrabFocus();
                break;
            case b.ReturnHomeScreen:
                Text = "Return Home";
                this.GrabFocus();
                break;
            case b.ReturnUpgrades:
                Text = "Return to Upgrades";
                this.GrabFocus();
                break;
            case b.StealthIce:
                Text = "Ice Stealth";
                break;
            case b.StealthFire:
                Text = "Fire Stealth";
                break;
            case b.PlusDino:
                Text = "+";
                break;
            case b.MinusDino:
                Text = "-";
                break;
            case b.BuyDinos:
                Text = "Purchase";
                break;
            case b.ContinueConquest:
                Text = "Continue Conquest";
                break;
        }
    }

    void OnButtonPressed()
    {
        SceneChanger scnChng = SceneChanger.Instance;
        switch (buttonMode)
        {
            case b.Quit:
                GetTree().Quit();
                break;
            case b.Play:
                scnChng.GoToScene("res://src/GUI/screens/HomeScreen.tscn");
                break;
            case b.RetryCombat:
                CombatInfo.Instance.Reset();
                scnChng.GoToScene("res://src/combat/CombatScreen.tscn");
                break;
            case b.RetryStealth:
                if (StealthInfo.findingIce)
                {
                    scnChng.GoToScene("res://src/stealth/StealthIce.tscn");
                }
                else
                {
                    scnChng.GoToScene("res://src/stealth/StealthFire.tscn");
                }
                break;
            case b.ReturnHomeScreen:
                scnChng.GoToScene("res://src/GUI/screens/HomeScreen.tscn");
                break;
            case b.ReturnUpgrades:
                scnChng.GoToScene("res://src/GUI/screens/UpgradeScreen.tscn");
                break;
            case b.StealthIce:
                StealthInfo.findingIce = true;
                StealthInfo.findingFire = false;
                scnChng.GoToScene("res://src/stealth/StealthIce.tscn");
                break;
            case b.StealthFire:
                StealthInfo.findingIce = false;
                StealthInfo.findingFire = true;
                scnChng.GoToScene("res://src/stealth/StealthFire.tscn");
                break;
            // nothing for plus, minus, or buy dinos: those are handled in their own scenes
            case b.ContinueConquest:
                Events.publishNewRound();
                break;
        }
    }

}