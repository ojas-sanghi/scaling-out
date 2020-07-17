extends Area2D

var speed = Vector2(-550, 0)

var bullet_type = ""
var bullet_dmg = 17

func _physics_process(delta: float) -> void:
	position += speed * delta

func _on_Timer_timeout() -> void:
	queue_free()

func _on_Bullet_area_entered(area: Area2D) -> void:
	Signals.emit_signal("dino_hit", 17)
	queue_free()
