extends GeneralDino

func _init() -> void:
	dino_type = Enums.dinos.mega

	deploy_delay_value = 2

	dino_unlock_cost = [10, 10]
	special_gene_type = ""

	.calculate_upgrades()
