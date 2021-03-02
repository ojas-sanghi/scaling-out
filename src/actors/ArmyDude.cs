using Godot;

public class ArmyDude : Area2D
{
    // TODO: convert this to an enum
    string mode;

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
                mode = "pistol";
                rps = 1;
                magSize = 15;
                reloadTime = 2;
                break;

            case 1:
                mode = "rifle";
                rps = 2;
                magSize = 20;
                reloadTime = 3;
                break;

            case 2:
                mode = "shotgun";
                rps = 1.2;
                magSize = 5;
                reloadTime = 2.5;
                break;
        }

        spawner.mode = mode;
        bulletsLeft = magSize;

        animPlayer.Play("shoot_" + mode);
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
        //? Does this work?
        //? Once types are changed to Enums, we don't need to do this
        string strType = type.ToString();

        if (strType == "ice")
        {
            animPlayer.PlaybackSpeed = (float)0.5;
            await ToSignal(GetTree().CreateTimer((float)10.0), "timeout");
            animPlayer.PlaybackSpeed = (float)1.0;
        }
        else if (strType == "fire")
        {
            animPlayer.Stop();
            await ToSignal(GetTree().CreateTimer((float)3), "timeout");
            animPlayer.Play("shoot_" + mode);
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
            animPlayer.Play("shoot_" + mode);
        }
    }
}