using Godot;

public class GameTimer : Control 
{
    string labelText = "Time left: ";

    Timer timer;
    Label label;

    [Export] int timerDuration = 120;


    public override void _Ready()
    {
        timer = (Timer)FindNode("Timer");
        label = (Label)FindNode("Label");

        timer.WaitTime = timerDuration;
        timer.Start();
    }

    void ResetTime() {
        timer.WaitTime = timerDuration;
    }

    public override void _Process(float delta)
    {
        // Get time left in seconds and round it to an int
        float timeLeftF = timer.TimeLeft;
        int timeLeft = Mathf.RoundToInt(timeLeftF);

        // Get minutes remaining
        int timeLeftM = Mathf.RoundToInt(timeLeft / 60);

        // Get seconds remaining
        int timeLeftS = Mathf.RoundToInt(timeLeft % 60);

        // Make str of remaining time: "3m 37s"
        string timeLeftStr = timeLeftM.ToString() + "m " + timeLeftS.ToString() + "s";

        // Make full text
        labelText = "Time left: " + timeLeftStr;

        // Assign new text
        label.Text = labelText;
    }

    //? do thios work
    void _on_Timer_timeout() {
        Events.publishConquestLost();
    }

}