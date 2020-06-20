extends Area2D

var speed = Vector2(600, 0)

var disabled = true

func _ready() -> void:
	self.hide()

func _physics_process(delta: float) -> void:
	if not disabled:
		self.show()
		position += speed * delta
