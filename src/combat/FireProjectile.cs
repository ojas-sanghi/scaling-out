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

        Array armySoldiers = GetTree().GetNodesInGroup("combat_army");

        
        // stop shooting for duration
        foreach (CombatArmySoldier soldier in armySoldiers)
            soldier.animPlayer.Stop();

        await ToSignal(GetTree().CreateTimer(duration), "timeout");

        foreach (CombatArmySoldier soldier1 in armySoldiers)
            soldier1.animPlayer.Play("shoot_" + soldier1.gunType.ToString().ToLower());

        QueueFree();
    }
}