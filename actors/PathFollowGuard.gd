extends Path2D

onready var base_guard = $Follow/BaseGuard
onready var forward = base_guard.get_node("Forward")
onready var backward = base_guard.get_node("Backward")

var walk_time = 4


func forward_tween():
	forward.interpolate_property($Follow, "unit_offset",
								0, 1, walk_time,
								forward.TRANS_LINEAR,
								forward.EASE_IN_OUT)
	base_guard.rotation_degrees = 0
	forward.start()


func backward_tween():
	backward.interpolate_property($Follow, "unit_offset",
								1, 0, walk_time,
								backward.TRANS_LINEAR,
								backward.EASE_IN_OUT)
	base_guard.rotation_degrees = 180
	backward.start()


func _ready() -> void:
	forward.connect("tween_completed", self, "_on_Forward_tween_completed")
	backward.connect("tween_completed", self, "_on_Backward_tween_completed")

	forward_tween()


func _on_Forward_tween_completed(object: Object, key: NodePath) -> void:
	backward_tween()


func _on_Backward_tween_completed(object: Object, key: NodePath) -> void:
	forward_tween()
