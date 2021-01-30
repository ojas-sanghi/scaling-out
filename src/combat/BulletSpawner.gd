extends Node2D

var bullet = preload("res://src/combat/Bullet.tscn")
var bullet_group = preload("res://src/combat/BulletsGroup.tscn")

var bullets = []

var mode

func new_bullet():
	var new = bullet.instance()
	new.mode = mode
	bullets.append(new)

func spawn_bullets():
	# reset list
	bullets = []

	# bullet and bullets_group speed are
	# both half of that in the singleton
	# thus roughly same as the value itself in the singleton, and
	# the positions of the bullet and the bulletsgroup are the same
	var bullets_group = bullet_group.instance()
	bullets_group.rotation_degrees = rand_range(-5, 5)

	# the position of the bullet and the BulletsGroup are the same
	if mode == "shotgun":
		# make new three bullets
		for _i in range(0, 3):
			new_bullet()
		bullets[0].rotation_degrees = 5
		bullets[2].rotation_degrees = -5
	else:
		# one new bullet
		new_bullet()

	for b in bullets:
		bullets_group.add_child(b)
	add_child(bullets_group)

	$AudioStreamPlayer.play()
