using Godot;
using b = Enums.GameButtonModes;

[Tool]
public class GameButton : Button
{
    [Export] public b buttonMode;

    public override void _Ready()
    {
        if (buttonMode == b.None)
        {
            GD.PushError("You must set buttonMode for GameButton!");
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
            case b.Ice:
                Text = "Ice Stealth";
                break;
            case b.Fire:
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

    //? work?
    void _on_Button_pressed()
    {
        SceneChanger s = SceneChanger.Instance;
        switch (buttonMode)
        {
            case b.Quit:
                GetTree().Quit();
                break;
            case b.Play:
                s.GoToScene("res://src/GUI/screens/HomeScreen.tscn");
                break;
            case b.RetryCombat:
                CombatInfo.Instance.Reset();
                s.GoToScene("res://src/combat/CombatScreen.tscn");
                break;
            case b.RetryStealth:
                if (StealthInfo.findingIce) {
                    s.GoToScene("res://src/stealth/StealthIce.tscn");
                } else {
                    s.GoToScene("res://src/stealth/StealthFire.tscn");
                }
                break;
            case b.ReturnHomeScreen:
                s.GoToScene("res://src/GUI/screens/HomeScreen.tscn");
                break;
            case b.ReturnUpgrades:
                s.GoToScene("res://src/GUI/screens/UpgradeScreen.tscn");
                break;
            case b.Ice:
                StealthInfo.findingIce = true;
                StealthInfo.findingFire = false;
                s.GoToScene("res://src/stealth/StealthIce.tscn");
                break;
            case b.Fire:
                StealthInfo.findingIce = false;
                StealthInfo.findingFire = true;
                s.GoToScene("res://src/stealth/StealthFire.tscn");
                break;
            // nothing for plus, minus, or buy dinos: those are handled in their own scenes
            case b.ContinueConquest:
                Events.publishNewRound();
                break;
        }   
    }

}