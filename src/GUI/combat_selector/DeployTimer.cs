using System;
using Godot;

public class DeployTimer : Control
{
    Enums.Dinos dinoId = 0;
    Timer dinoTimer;
    TextureProgress progress;
    Tween tween;

    public override void _Ready()
    {
        Events.dinoDeployed += OnDinoDeployed;

        dinoTimer = (Timer)FindNode("Timer");
        progress = (TextureProgress)FindNode("TextureProgress");
        tween = (Tween)FindNode("Tween");

        // give the circle an id relating to each dino in the list
        foreach (Enums.Dinos d in Enum.GetValues(typeof(Enums.Dinos)))
        {
            if (CombatInfo.Instance.selectorTimerList.Contains(d))
            {
                continue;
            }
            CombatInfo.Instance.selectorTimerList.Add(d);
            dinoId = d;
            break;
        }
        progress.Hide();
    }

    public override void _ExitTree()
    {
        Events.dinoDeployed -= OnDinoDeployed;
    }

    void OnDinoDeployed(Enums.Dinos dinoType)
    {
        // only bother if the dino being deployed is our associated ID
        if (dinoType != dinoId)
        {
            return;
        }

        double delay = DinoInfo.Instance.GetDinoTimerDelay(dinoType);
        dinoTimer.Start((float)delay);

        UpdateProgressBar(dinoType);
    }

    void UpdateProgressBar(Enums.Dinos dinoType)
    {
        progress.Show();

        double delay = DinoInfo.Instance.GetDinoTimerDelay(dinoType);
        tween.InterpolateProperty(
            progress, "value", 0, 100, (float)delay
        );
        tween.Start();
    }

    void OnTimerTimeout()
    {
        progress.Hide();
    }
}