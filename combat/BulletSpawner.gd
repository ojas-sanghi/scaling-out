extends Node2D

var bullet = preload("res://combat/Bullet.tscn")
var bullets = []

var bullet_spawn_pos = [Vector2(0, 0)]
var bullet_sprays = [Vector2(0, -25), Vector2(0, -20), Vector2(0, 0), Vector2(0, 20), Vector2(0, 25)]

var mode

func check_mode() -> void:
	match mode:
		"shotgun":
			bullet_spawn_pos = [Vector2(-5, -20), Vector2(0, 0), Vector2(-5, 20)]
			bullet_sprays = [Vector2(0, 0)]

func spawn_bullets():
	check_mode()

	for b in bullet_spawn_pos:
		var new_bullet = bullet.instance()
		new_bullet.mode = mode

		new_bullet.position = b
		randomize()
		var spray_num = randi() % bullet_sprays.size()
		new_bullet.position += bullet_sprays[spray_num]

		bullets.append(new_bullet)

	match mode:
		"shotgun":
			bullets[0].rotation_degrees = 20
			bullets[2].rotation_degrees = -20

	for bullet in bullets:
		add_child(bullet)

	$AudioStreamPlayer.play()

# called by child bullet when it is despawning
# removes bullet from list so that we can still set the appropriate rotation degrees
func remove_bullet(b):
	bullets.remove(bullets.find(b))
