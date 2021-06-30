using Godot;

public class IceProjectile : DinoProjectile
{
    float duration = 10f;

    public override void _Ready()
    {
        if (isEnhanced) duration = 15f;
    }

    async public override void OnDinoProjectileAreaEntered(Area2D area)
    {
        if (disabled) return;
        base.OnDinoProjectileAreaEntered(area);

        var armySoliders = GetTree().GetNodesInGroup("combat_army");
        
        // make them shoot half as fast
        foreach (CombatArmySoldier soldier in armySoliders)
            soldier.animPlayer.PlaybackSpeed = 0.5f;
        
        await ToSignal(GetTree().CreateTimer(duration), "timeout");

        foreach (CombatArmySoldier soldier1 in armySoliders)
            soldier1.animPlayer.PlaybackSpeed = 1f;

        QueueFree();
    }

}