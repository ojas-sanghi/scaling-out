extends GeneralDino


func _init() -> void:
	dino_name = "tanky"
	dino_variations = ["blue", "orange", "pink"]

	dino_speed = Vector2(35, 0)
	dino_health = 200
	dino_dmg = 2
	dino_defense = 1
	spawn_delay = 2
	deploy_delay = 2

	dino_gene_cost = 250
	special_gene_type = "ice"


func _ready() -> void:
	$IceProjectile.hide()


func shoot_projectile():
	$IceProjectile.show()
	$IceProjectile.disabled = false
