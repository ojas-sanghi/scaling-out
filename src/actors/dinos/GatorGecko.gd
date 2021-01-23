extends GeneralDino

func _init() -> void:
	dino_type = Enums.dinos.gator

	deploy_delay_value = 2

	dino_unlock_cost = [50, 50]
	special_gene_type = "florida"

	.calculate_upgrades()