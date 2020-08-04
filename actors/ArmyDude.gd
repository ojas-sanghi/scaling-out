extends Area2D

var mode

var rps
var mag_size
var reload_time

var bullets_left

func _ready() -> void:
	randomize()
	var random = randi() % 3
	match random:
		0:
			mode = "pistol"
			rps = 1
			mag_size = 15
			reload_time = 2
		1:
			mode = "rifle"
			rps = 2
			mag_size = 20
			reload_time = 3
		2:
			mode = "shotgun"
			rps = 1.2
			mag_size = 5
			reload_time = 2.5
	$BulletSpawner.mode = mode

	bullets_left = mag_size

	$AnimationPlayer.play("shoot_" + mode)

	Signals.connect("proj_hit", self, "_on_proj_hit")



func _on_proj_hit(type):
	if type == "ice":
		$AnimationPlayer.current_animation_length = 2
		yield(get_tree().create_timer(10), "timeout")
		$AnimationPlayer.current_animation_length = 1
	elif type == "fire":
		$AnimationPlayer.stop()
		yield(get_tree().create_timer(3), "timeout")
		$AnimationPlayer.play("shoot_" + mode)


func check_reload() -> void:
	bullets_left -= rps

	if bullets_left <= 0:
		$AnimationPlayer.stop()
		# play reload anim

		yield(get_tree().create_timer(reload_time), "timeout")
		bullets_left = mag_size
		$AnimationPlayer.play("shoot_" + mode)
