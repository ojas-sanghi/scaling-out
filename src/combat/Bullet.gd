extends Area2D

var mode = "pistol"

var speed := CombatInfo.bullet_speed / 2
var bullet_dmg = 14

func _ready() -> void:

	if mode == "shotgun":
		bullet_dmg = 6
		$ExistenceTimer.wait_time = 0.75
	else:
		bullet_dmg = 4
		bullet_dmg = 4

	$ExistenceTimer.start()

func _physics_process(delta: float) -> void:
	var bullet_vel = Vector2(-1, 0).rotated(rotation) * speed
	position += bullet_vel * delta

func _on_Bullet_area_entered(dino: GeneralDino) -> void:
	dino.update_health(bullet_dmg)
	queue_free()

func _on_ExistenceTimer_timeout() -> void:
	bullet_dmg = 1

