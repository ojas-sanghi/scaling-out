using Enums;
using Godot;

public class ArmyDude : Area2D
{
    ArmyGunTypes mode;

    double rps;
    int magSize;
    float reloadTime;

    int bulletsLeft;

    AnimationPlayer animPlayer;
    RayCast2D rayCast;

    public override void _Ready()
    {
        BulletSpawner spawner = (BulletSpawner)FindNode("BulletSpawner");
        animPlayer = (AnimationPlayer)FindNode("AnimationPlayer");
        rayCast = (RayCast2D)FindNode("RayCast2D");

        GD.Randomize();
        uint random = GD.Randi() % 3;

        switch (random)
        {
            case 0:
                mode = ArmyGunTypes.Pistol;
                rps = 1;
                magSize = 15;
                reloadTime = 2f;
                break;

            case 1:
                mode = ArmyGunTypes.Rifle;
                rps = 2;
                magSize = 20;
                reloadTime = 3f;
                break;

            case 2:
                mode = ArmyGunTypes.Shotgun;
                rps = 1.2;
                magSize = 5;
                reloadTime = 2.5f;
                break;
        }

        spawner.mode = mode;
        bulletsLeft = magSize;

        //? what happens when you cast enum to strin    

        animPlayer.Play("shoot_" + mode.ToString().ToLower());
        animPlayer.Seek(0.1f, true);

        rayCast.CastTo = new Vector2(-GetViewport().Size.x, 0);

        Events.projectileHit += OnProjectileHit;
    }

    public override void _ExitTree()
    {
        Events.projectileHit -= OnProjectileHit;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (rayCast.IsColliding())
        {
            animPlayer.Play();
        }
        else
        {
            animPlayer.Stop();
        }
    }

    async void OnProjectileHit(Enums.Genes type)
    {
        if (type == Enums.Genes.Ice)
        {
            animPlayer.PlaybackSpeed = 0.5f;
            await ToSignal(GetTree().CreateTimer(10f), "timeout");
            animPlayer.PlaybackSpeed = 1f;
        }
        else if (type == Enums.Genes.Fire)
        {
            animPlayer.Stop();
            await ToSignal(GetTree().CreateTimer(3f), "timeout");
            animPlayer.Play("shoot_" + mode.ToString().ToLower());
        }

    }

    async void CheckReload()
    {
        //? how does this work
        //? remove the (int) maybe
        bulletsLeft -= (int)rps;

        if (bulletsLeft <= 0)
        {
            animPlayer.Stop();

            // play reload anim
            await ToSignal(GetTree().CreateTimer(reloadTime), "timeout");
            bulletsLeft = magSize;
            animPlayer.Play("shoot_" + mode.ToString().ToLower());
        }
    }
}