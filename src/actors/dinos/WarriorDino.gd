extends GeneralDino


func _init() -> void:
	dino_type = Enums.dinos.warrior

	deploy_delay_value = 2

	dino_unlock_cost = [50, 60]
	special_gene_type = "fire"

	.calculate_upgrades()


func _ready() -> void:
	$FireProjectile.hide()


func shoot_projectile():
	$FireProjectile.show()
	$FireProjectile.disabled = false
