extends Area2D

func _ready() -> void:
	randomize()
	var random = randi() % 3
	var weapon = "rifle"
	match random:
		0:
			weapon = "rifle"
		1:
			weapon = "handgun"
		2:
			weapon = "shotgun"
	var anim_string = "move_" + weapon
	$AnimatedSprite.play(anim_string)


func _on_BaseGuard_body_entered(body: Node) -> void:
	var rotate_deg = rad2deg(position.angle_to(body.position))
	rotation_degrees += rotate_deg
	Signals.emit_signal("level_failed")

