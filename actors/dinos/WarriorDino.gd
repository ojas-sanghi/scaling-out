extends GeneralDino

func _init() -> void:
	dino_name = "warrior"
	dino_variations = ["green", "purple", "red"]

	dino_speed = Vector2(100, 0)
	dino_health = 45
	dino_dmg = 10
	dino_defense = 1
	spawn_delay = 2
	deploy_delay = 2

	dino_gene_cost = 350
	special_gene_type = "fire"

func _ready() -> void:
	$FireProjectile.hide()

func shoot_projectile():
	$FireProjectile.show()
	$FireProjectile.disabled = false
