extends GeneralDino

func _init() -> void:
	dino_name = "mega"

	deploy_delay = 2

	dino_cred_cost = 10
	dino_unlock_cost = [10, 10]
	special_gene_type = ""

	.calculate_upgrades()
