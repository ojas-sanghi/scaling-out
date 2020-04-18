extends Area2D

var speed = Vector2(-550, 0)

func _physics_process(delta: float) -> void:
	position += speed * delta

func _on_Timer_timeout() -> void:
	queue_free()
