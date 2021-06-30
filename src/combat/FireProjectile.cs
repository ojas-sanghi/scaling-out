using Godot;
using Godot.Collections;

public class FireProjectile : DinoProjectile
{
    float duration = 5f;

    public override void _Ready()
    {
        if (isEnhanced) duration = 10f;
    }

    async public override void OnDinoProjectileAreaEntered(Area2D area)
    {
        if (disabled) return;
        base.OnDinoProjectileAreaEntered(area);

        Array armySoliders = GetTree().GetNodesInGroup("combat_army");

        
        // stop shooting for duration
        foreach (CombatArmySoldier soldier in armySoliders)
            soldier.animPlayer.Stop();

        await ToSignal(GetTree().CreateTimer(duration), "timeout");

        foreach (CombatArmySoldier soldier1 in armySoliders)
            soldier1.animPlayer.Play("shoot_" + soldier1.mode.ToString().ToLower());

        QueueFree();
    }
}