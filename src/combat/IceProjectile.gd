extends Area2D

var speed = Vector2(600, 0)

var disabled = true


func _ready() -> void:
	self.hide()


func _physics_process(delta: float) -> void:
	if not disabled:
		self.show()
		position += speed * delta


func _on_IceProjectile_area_entered(area: Area2D) -> void:
	if not disabled:
		Signals.emit_signal("proj_hit", "ice")
