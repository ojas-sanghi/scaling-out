extends GeneralDino

func _init() -> void:
	dino_name = "mega"
	dino_variations = ["blue", "green", "orange"]

	dino_speed = Vector2(50, 0)

	dino_health = 100
	dino_dmg = 5
	dino_defense = 1
	spawn_delay = 2
	deploy_delay = 2

	dino_gene_cost = 0
	special_gene_type = ""
