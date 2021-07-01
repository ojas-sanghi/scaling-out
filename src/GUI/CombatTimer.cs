using Godot;

public class CombatTimer : Control
{
    string labelText = "Time left: ";

    public Timer timer;
    Label label;

    public int timerDuration = 0;

    public override void _Ready()
    {
        timer = (Timer)FindNode("Timer");
        label = (Label)FindNode("Label");

        StartTimer();

        Events.newRound += StartTimer;
    }

    public override void _ExitTree()
    {
        Events.newRound -= StartTimer;
    }

    public void StartTimer()
    {
        int timerDuration = CityInfo.Instance.currentCity.timePerRoundSeconds[CombatInfo.Instance.currentRound - 1];
        timer.Start(timer.TimeLeft + timerDuration);
    }

    public override void _Process(float delta)
    {
        // Get time left in seconds and round it to an int
        float timeLeftF = timer.TimeLeft;
        int timeLeft = (int)Mathf.Round(timer.TimeLeft);

        // Get minutes remaining
        int timeLeftM = (int)timeLeft / 60;

        // Get seconds remaining
        int timeLeftS = (int)timeLeft % 60;

        // Make str of remaining time: "3m 37s"
        string timeLeftStr = timeLeftM.ToString() + "m " + timeLeftS.ToString() + "s";

        // Make full text
        labelText = "Time left: " + timeLeftStr;

        // Assign new text
        label.Text = labelText;
    }

    void OnTimerTimeout()
    {
        Events.publishConquestLost();
    }

}