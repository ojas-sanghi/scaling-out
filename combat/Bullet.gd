extends Area2D

var mode = "pistol"

var speed = Vector2(-550, 0)

var bullet_dmg = 14


func _ready() -> void:
	match mode:
		"pistol":
			bullet_dmg = 4
		"rifle":
			bullet_dmg = 4
		"shotgun":
			bullet_dmg = 6
			$ExistenceTimer.wait_time = 0.75
	$ExistenceTimer.start()

func _physics_process(delta: float) -> void:
	position += speed * delta


func _on_Bullet_area_entered(dino: Area2D) -> void:
	dino.update_health(bullet_dmg)
	get_parent().remove_bullet(self)
	queue_free()


func _on_ExistenceTimer_timeout() -> void:
	get_parent().remove_bullet(self)
	queue_free()

