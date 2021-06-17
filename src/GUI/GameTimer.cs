using Godot;

public class GameTimer : Control
{
    string labelText = "Time left: ";

    Timer timer;
    Label label;

    public int timerDuration = 0;


    public override void _Ready()
    {
        timer = (Timer)FindNode("Timer");
        label = (Label)FindNode("Label");

        Events.newRound += OnNewRound;
    }

    public void StartTimer()
    {
        timer.Start(timerDuration);
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

    void OnNewRound()
    {
        // restart Timer with more time in a new round
        timer.Start(timer.TimeLeft + timerDuration);
    }

    void OnTimerTimeout()
    {
        Events.publishConquestLost();
    }

}