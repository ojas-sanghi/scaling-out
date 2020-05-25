extends Area2D

var speed = Vector2(600, 0)

var disabled = true

func _physics_process(delta: float) -> void:
	if not disabled:
		position += speed * delta
