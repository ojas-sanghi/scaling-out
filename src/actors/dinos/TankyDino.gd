extends GeneralDino


func _init() -> void:
	dino_type = Enums.dinos.tanky

	deploy_delay_value = 2

	dino_unlock_cost = [25, 50]
	special_gene_type = "ice"

	.calculate_upgrades()


func _ready() -> void:
	$IceProjectile.hide()


func shoot_projectile():
	$IceProjectile.show()
	$IceProjectile.disabled = false
