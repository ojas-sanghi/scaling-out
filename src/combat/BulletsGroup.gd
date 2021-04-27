extends Area2D

var speed := CombatInfo.bullet_speed / 2

func _physics_process(delta: float) -> void:
	var velocity = Vector2(-1, 0).rotated(rotation) * speed
	position += velocity * delta

func _on_VisibilityNotifier2D_screen_exited() -> void:
	queue_free()
