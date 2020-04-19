extends Path2D

onready var forward = $Follow/Area2D/Forward
onready var backward = $Follow/Area2D/Backward
var walk_time = 4

signal level_failed

func forward_tween():
	forward.interpolate_property($Follow, "unit_offset",
								0, 1, walk_time,
								forward.TRANS_LINEAR,
								forward.EASE_IN_OUT)
	$Follow/Area2D.rotation_degrees = 0
	forward.start()

func backward_tween():
	backward.interpolate_property($Follow, "unit_offset",
								1, 0, walk_time,
								backward.TRANS_LINEAR,
								backward.EASE_IN_OUT)
	$Follow/Area2D.rotation_degrees = 180
	backward.start()

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
	$Follow/Area2D/AnimatedSprite.play(anim_string)
	forward_tween()

func _on_Forward_tween_completed(object: Object, key: NodePath) -> void:
	backward_tween()

func _on_Backward_tween_completed(object: Object, key: NodePath) -> void:
	forward_tween()

func stop_all_anim():
	var guards = get_tree().get_nodes_in_group("guards")
	if guards:
		for g in guards:
			g.stop_anim()

func stop_anim():
	$Follow/Area2D/AnimatedSprite.playing = false

func _on_FieldOfView_enemy_spotted() -> void:
	stop_all_anim()
	emit_signal("level_failed")

func _on_Area2D_body_entered(body: Node) -> void:
	stop_all_anim()
	var rotate_deg = rad2deg(position.angle_to(body.position))
	rotation_degrees += rotate_deg
	if body.name == "Player":
		emit_signal("level_failed")

func stop_moving():
	$Follow/Area2D/AnimatedSprite.stop()
