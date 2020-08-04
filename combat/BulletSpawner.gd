extends Node2D

var bullet = preload("res://combat/Bullet.tscn")
var bullets = []
var bullet_spawn_pos = [Vector2(0, 0)]

var mode

func check_mode() -> void:
	match mode:
		"shotgun":
			bullet_spawn_pos = [Vector2(-5, -10), Vector2(0, 0), Vector2(-5, 10)]

func spawn_bullets():
	check_mode()


	var bullets_to_be_spawned = []
	for b in bullet_spawn_pos:
		var new_bullet = bullet.instance()
		new_bullet.position = b
		new_bullet.mode = mode

		add_child(new_bullet)
		bullets.append(new_bullet)


	$AudioStreamPlayer.play()
