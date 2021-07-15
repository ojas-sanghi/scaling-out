using Godot;
using b = Enums.GameButtonModes;

[Tool]
public class GameButton : Button
{
    [Export] public b buttonMode;

    PackedScene newMap;

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

        SetButtonText();
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
            case b.ReturnUpgradeSelect:
                Text = "Return";
                this.GrabFocus();
                break;
            case b.StealthSelectScreen:
                Text = "Go to Stealth";
                break;
            case b.ContinueConquest:
                Text = "Continue Conquest";
                break;
            case b.EasyStealthMap:
                Text = "EASY MAP";
                break;
            case b.MediumStealthMap:
                Text = "MEDIUM MAP";
                break;
            case b.HardStealthMap:
                Text = "HARD MAP";
                break;
            case b.GeneStealthMap:
                Text = "RANDOM GENE MAP";
                if (StealthInfo.Instance.GetUnbeatenGeneMap() == null)
                    Disabled = true;
                break;
        }
    }

    void OnButtonPressed()
    {
        SceneChanger scnChng = SceneChanger.Instance;
        StealthInfo s = StealthInfo.Instance;

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
                scnChng.GoToPackedScene(StealthInfo.currentStealthMap);
                break;
            case b.ReturnHomeScreen:
                scnChng.GoToScene("res://src/GUI/screens/HomeScreen.tscn");
                break;
            case b.ReturnUpgradeSelect:
                scnChng.GoToScene("res://src/GUI/screens/DinoUpgradeSelectScreen.tscn");
                break;
            case b.StealthSelectScreen:
                scnChng.GoToScene("res://src/GUI/screens/StealthSelectScreen.tscn");
                break;
            // nothing for plus, minus, or buy dinos: those are handled in their own scenes
            case b.ContinueConquest:
                scnChng.NewRoundAnimation();
                break;
            case b.EasyStealthMap:
                newMap = StealthInfo.Instance.GetNormalMap(Enums.StealthMapDifficultyLevel.Easy);
                StealthInfo.currentStealthMap = newMap;
                scnChng.GoToPackedScene(newMap);
                break;
            case b.MediumStealthMap:
                newMap = StealthInfo.Instance.GetNormalMap(Enums.StealthMapDifficultyLevel.Medium);
                StealthInfo.currentStealthMap = newMap;
                scnChng.GoToPackedScene(newMap);
                break;
            case b.HardStealthMap:
                newMap = StealthInfo.Instance.GetNormalMap(Enums.StealthMapDifficultyLevel.Hard);
                StealthInfo.currentStealthMap = newMap;
                scnChng.GoToPackedScene(newMap);
                break;
            case b.GeneStealthMap:
                scnChng.GoToPackedScene(StealthInfo.Instance.GetUnbeatenGeneMap());
                break;
        }
    }
}