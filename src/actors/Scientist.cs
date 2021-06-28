using Godot;
using Godot.Collections;
using System;
using System.Linq;

public class Scientist : KinematicBody2D
{
    [Export] int speed = 300;

    AnimatedSprite animSprite;

    Vector2 velocity;
    int coinsCollected = 0;

    string nodePath = "";

    public override void _Ready()
    {
        animSprite = (AnimatedSprite)FindNode("AnimatedSprite");

        Events.levelFailed += OnLevelFailed;
        Events.levelPassed += OnLevelPassed;
        Events.coinGrabbed += OnCoinGrabbed;

        // TODO: when more gene-specific scenes are made, change this
        string geneString = StealthInfo.geneBeingPursued.ToString();
        nodePath = "/root/Stealth" + geneString.First().ToString().ToUpper() + geneString.Substring(1) + "/";
    }

    public override void _ExitTree()
    {
        Events.levelFailed -= OnLevelFailed; //? do we remove it here and in OnLevelFailed? Do we need a check here?
        Events.levelPassed -= OnLevelPassed;
        Events.coinGrabbed -= OnCoinGrabbed;
    }


    Vector2 GetInput()
    {
        // Detect up/down/left/right keystrokes and move only when pressed
        var newVelocity = Vector2.Zero;
        if (Input.IsActionPressed("ui_right"))
        {
            newVelocity.x++;
        }
        if (Input.IsActionPressed("ui_left"))
        {
            newVelocity.x--;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            newVelocity.y++;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            newVelocity.y--;
        }

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

        await SceneChanger.Instance.Fade();
        GetNode<CanvasModulate>(nodePath + "CanvasModulate").Hide();
        GetNode<MoneyCounter>(nodePath + "CanvasLayer/CoinCounter").Hide();
        await ToSignal(win, "finished");

        GetNode<Vault>(nodePath + "Vault").Hide();
        PlayerStats.gold += coinsCollected;
        SceneChanger.Instance.GoToScene("res://src/GUI/dialogues/StealthWinDialogue.tscn");
    }

    async void OnLevelFailed()
    {
        // remove here since we want it to be one-shot
        Events.levelFailed -= OnLevelFailed;

        GD.Print("YOU FAILED!");
        
        var caught = (AudioStreamPlayer) FindNode("Caught");

        SetPhysicsProcess(false);
        animSprite.Stop();
        caught.Play();

        await SceneChanger.Instance.Fade();
        GetNode<CanvasModulate>(nodePath + "CanvasModulate").Hide();
        GetNode<MoneyCounter>(nodePath + "CanvasLayer/CoinCounter").Hide();
        await ToSignal(caught, "finished");

        GetNode<Vault>(nodePath + "Vault").Hide();
        SceneChanger.Instance.GoToScene("res://src/GUI/dialogues/StealthLoseDialogue.tscn");
    }

    void OnCoinGrabbed(int value)
    {
        coinsCollected += value;
    }

//     public override void _Draw()
//     {
//         FOV();
//     }

//     void GetOccluderPolygons()
//     {
//         var occluders = GetTree().GetNodesInGroup("fov_occluders").Cast<Node>();
//         var polygons;
//         foreach (var o in occluders)
//         {
//             var mask = (Polygon2D)o.GetNode("occlusion_mask");
//             if (mask != null)
//             {
//                 var poly = mask.Polygon;
//                 for 
//             }
//         }
//     }

//     void FOV() 
//     {

//     }

//     func _draw():
// 	fov()

// func get_occluder_polygons():
// 	var occluders = get_tree().get_nodes_in_group('fov_occluders')
// 	var polygons = []
// 	for o in occluders:
// 		var mask = o.get_node('occlusion_mask')
// 		if mask != null:
// 			var poly = mask.polygon
// 			for i in range(len(poly)):
// 				poly[i] += (o.position - self.position)
// 			polygons.append(poly)
// 	return polygons

// func fov():
// 	var fov_computer = get_node('FOV')
// 	var polygons = get_occluder_polygons()
// 	var fov_center = Vector2(0,0)
// 	var fov_poly = fov_computer.get_fov_from_polygons(polygons, fov_center)
// 	draw_colored_polygon(fov_poly, Color(0,0,255))



}