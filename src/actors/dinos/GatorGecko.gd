extends GeneralDino

func _init() -> void:
	dino_type = Enums.dinos.gator

	dino_unlock_cost = [50, 50]
	special_gene_type = "florida"

	.calculate_upgrades()
