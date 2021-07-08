using Godot;

public class Scientist : KinematicBody2D
{
    [Export] int speed = 300;

    AnimatedSprite animSprite;

    Vector2 velocity;
    int coinsCollected = 0;

    Node2D rootStealthNode;

    public override void _Ready()
    {
        animSprite = (AnimatedSprite)FindNode("AnimatedSprite");

        Events.levelFailed += OnLevelFailed;
        Events.levelPassed += OnLevelPassed;
        Events.coinGrabbed += OnCoinGrabbed;

        rootStealthNode = (Node2D)GetParent();
    }

    public override void _ExitTree()
    {
        Events.levelFailed -= OnLevelFailed;
        Events.levelPassed -= OnLevelPassed;
        Events.coinGrabbed -= OnCoinGrabbed;
    }


    Vector2 GetInput()
    {
        // Detect up/down/left/right keystrokes and move only when pressed
        var newVelocity = Vector2.Zero;

        if (Input.IsActionPressed("ui_right"))
            newVelocity.x++;
        
        if (Input.IsActionPressed("ui_left"))
            newVelocity.x--;
        
        if (Input.IsActionPressed("ui_down"))
            newVelocity.y++;
        
        if (Input.IsActionPressed("ui_up"))
            newVelocity.y--;

        return newVelocity;
    }

    void AnimatePlayer()
    {
        float velocityLength = velocity.Length();
        float velocityAngle = Mathf.Rad2Deg(velocity.Angle());

        // Add 90 degrees since otherwise it treats going right as 0 degrees
        velocityAngle += 90;

        // If we're moving, change rotation
        if (velocityLength >= 1)
        {
            //$AnimatedSprite.rotation_degrees = velocity_angle
            //$Collision.rotation_degrees = velocity_angle
            RotationDegrees = velocityAngle - 180;

            // Play walk animation if going anywhere, else idle
            animSprite.Play("walk");
        }
        else
        {
            animSprite.Play("idle");
        }

    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = GetInput();
        velocity = velocity.Normalized() * speed;
        AnimatePlayer();
        MoveAndSlide(velocity);
    }

    async void OnLevelPassed()
    {
        var win = (AudioStreamPlayer)FindNode("Win");

        SetPhysicsProcess(false);
        animSprite.Stop();
        win.Play();

        await SceneChanger.Instance.FadeOut();
        rootStealthNode.GetNode<CanvasModulate>("CanvasModulate").Hide();
        rootStealthNode.GetNode<MoneyCounter>("CanvasLayer/CoinCounter").Hide();
        PlayerStats.gold += coinsCollected;
        await ToSignal(win, "finished");
        
        rootStealthNode.GetNode<Vault>("Vault").Hide();
        SceneChanger.Instance.GoToScene("res://src/GUI/dialogues/StealthWinDialogue.tscn");
    }

    async void OnLevelFailed()
    {
        // remove here since we want it to be one-shot
        Events.levelFailed -= OnLevelFailed;

        var caught = (AudioStreamPlayer)FindNode("Caught");

        SetPhysicsProcess(false);
        animSprite.Stop();
        caught.Play();

        await SceneChanger.Instance.FadeOut();
        rootStealthNode.GetNode<CanvasModulate>("CanvasModulate").Hide();
        rootStealthNode.GetNode<MoneyCounter>("CanvasLayer/CoinCounter").Hide();
        await ToSignal(caught, "finished");

        rootStealthNode.GetNode<Vault>("Vault").Hide();
        SceneChanger.Instance.GoToScene("res://src/GUI/dialogues/StealthLoseDialogue.tscn");
    }

    void OnCoinGrabbed(int value)
    {
        coinsCollected += value;
    }
}