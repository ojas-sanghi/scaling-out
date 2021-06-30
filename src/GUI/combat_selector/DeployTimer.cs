using Godot;

public class DeployTimer : Control
{
    public Enums.Dinos dinoType = Enums.Dinos.None;
    Timer dinoTimer;
    TextureProgress progress;
    Tween tween;

    public override void _Ready()
    {
        Events.dinoDeployed += OnDinoDeployed;

        dinoTimer = (Timer)FindNode("Timer");
        progress = (TextureProgress)FindNode("TextureProgress");
        tween = (Tween)FindNode("Tween");

        progress.Hide();
    }

    public override void _ExitTree()
    {
        Events.dinoDeployed -= OnDinoDeployed;
    }

    void OnDinoDeployed(Enums.Dinos _dinoType)
    {
        // only bother if the dino being deployed is our associated ID
        if (_dinoType != this.dinoType)
        {
            return;
        }

        float delay = DinoInfo.Instance.GetDinoTimerDelay(dinoType);
        dinoTimer.Start(delay);

        UpdateProgressBar(dinoType);
    }

    void UpdateProgressBar(Enums.Dinos dinoType)
    {
        progress.Show();

        float delay = DinoInfo.Instance.GetDinoTimerDelay(dinoType);
        tween.InterpolateProperty(
            progress, "value", 0, 100, delay
        );
        tween.Start();
    }

    void OnTimerTimeout()
    {
        progress.Hide();
    }
}