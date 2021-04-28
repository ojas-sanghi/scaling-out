using Godot;
using Enums;

public class ArmyDude : Area2D
{
    ArmyWeapons mode;

    double rps;
    int magSize;
    double reloadTime;

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
                mode = ArmyWeapons.Pistol;
                rps = 1;
                magSize = 15;
                reloadTime = 2;
                break;

            case 1:
                mode = ArmyWeapons.Rifle;
                rps = 2;
                magSize = 20;
                reloadTime = 3;
                break;

            case 2:
                mode = ArmyWeapons.Shotgun;
                rps = 1.2;
                magSize = 5;
                reloadTime = 2.5;
                break;
        }

        spawner.mode = mode;
        bulletsLeft = magSize;

        //? what happens when you cast enum to strin    

        animPlayer.Play("shoot_" + mode.ToString().ToLower());
        animPlayer.Seek((float)0.1, true);

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
        string strType = type.ToString();

        if (strType == "Ice")
        {
            animPlayer.PlaybackSpeed = (float)0.5;
            await ToSignal(GetTree().CreateTimer((float)10.0), "timeout");
            animPlayer.PlaybackSpeed = (float)1.0;
        }
        else if (strType == "Fire")
        {
            animPlayer.Stop();
            await ToSignal(GetTree().CreateTimer((float)3), "timeout");
            animPlayer.Play("shoot_" + mode.ToString().ToLower());
        }

    }

    // TODO: fix animation playuer going to wrong function
    async void CheckReload()
    {
        //? how does this work
        //? remove the (int) maybe
        bulletsLeft -= (int)rps;

        if (bulletsLeft <= 0)
        {
            animPlayer.Stop();

            // play reload anim
            await ToSignal(GetTree().CreateTimer((float)reloadTime), "timeout");
            bulletsLeft = magSize;
            animPlayer.Play("shoot_" + mode.ToString().ToLower());
        }
    }
}