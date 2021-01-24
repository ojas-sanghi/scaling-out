extends Node

var dino_cred_cost = 30

var dino_list := {
	Enums.dinos.mega: preload("res://src/actors/dinos/MegaDino.tscn"),
	Enums.dinos.tanky: preload("res://src/actors/dinos/TankyDino.tscn"),
	Enums.dinos.warrior: preload("res://src/actors/dinos/WarriorDino.tscn"),
	Enums.dinos.gator: preload("res://src/actors/dinos/GatorGecko.tscn"),
}

var dino_icons := {
	Enums.dinos.mega: preload("res://assets/dinos/mega_dino/mega_dino.png"),
	Enums.dinos.tanky: preload("res://assets/dinos/tanky_dino/Armored_Dino_ICON.png"),
	Enums.dinos.warrior: preload("res://assets/dinos/warrior_dino/Tribal_Dino_icon.png"),
	Enums.dinos.gator: preload("res://assets/dinos/gator_gecko/gater_gecko_icon.png"),
}

var dino_ability_icons = {
	Enums.dinos.mega: "",
	Enums.dinos.tanky: preload("res://assets/dinos/misc/ice.png"),
	Enums.dinos.warrior: preload("res://assets/dinos/misc/fire.png"),
	Enums.dinos.gator: ""
}

class UpgradeInfo:
	var stats: Dictionary

	func _init(path) -> void:
		var data = load(path)

		stats = {
			Enums.stats.hp: data.hp_stat,
			Enums.stats.delay: data.delay_stat,
			Enums.stats.def: data.def_stat,
			Enums.stats.dodge: data.dodge_stat,
			Enums.stats.dmg: data.dmg_stat,
			Enums.stats.speed:data.speed_stat,
			Enums.stats.special: data.special_stat
		}

	func upgrade(stat: int) -> void:
		if not is_maxed_out(stat):
			stats[stat].level += 1

	func get_stat(stat: int):
		return stats[stat].get_stat()
	func get_level(stat: int) -> float:
		return stats[stat].level

	func get_gold_cost(stat: int) -> int:
		return stats[stat].get_gold()
	func get_gene_cost(stat: int) -> int:
		return stats[stat].get_genes()

	# if the user has paid and unlocked the special
	func unlocked_special() -> bool:
		return get_level(Enums.stats.special) == 1
	# if the dino has a special u can unlock
	func has_special() -> bool:
		return get_stat(Enums.stats.special) != ""


	## misc utlity functions
	func get_max_level(stat: int) -> int:
		if stat == Enums.stats.special:
			return 1 if has_special() else 0
		else:
			# our level is 0-indexed but size() is not, so decrement one
			return stats[stat].stats.size() - 1

	# [gold, genes]
	func get_next_upgrade_cost(stat: int) -> Array:
		var current_level = get_level(stat)

		# no cost if already at max
		if is_maxed_out(stat):
			return [0, 0]

		var gold_cost = stats[stat].get_gold(current_level + 1)
		var gene_cost = stats[stat].get_genes(current_level + 1)
		return [gold_cost, gene_cost]

	func is_maxed_out(stat: int) -> bool:
		return get_level(stat) >= get_max_level(stat)

var upgrades_info := {
	Enums.dinos.mega: UpgradeInfo.new("res://src/actors/dinos/stats/MegaDino.tres"),
	Enums.dinos.tanky: UpgradeInfo.new("res://src/actors/dinos/stats/TankyDino.tres"),
	Enums.dinos.warrior: UpgradeInfo.new("res://src/actors/dinos/stats/WarriorDino.tres"),
	Enums.dinos.gator: UpgradeInfo.new("res://src/actors/dinos/stats/GatorDino.tres"),
}

func get_dino(dino: int) -> UpgradeInfo:
	return upgrades_info[dino]

func get_dino_timer_delay():
	# find the delay based on the dino
	return get_dino_property("spawn_delay_value")

func get_dino_property(prop: String):
	# instance it, get the variable we want, then remove it
	var dino_scene = dino_list[CombatInfo.dino_id]
	var dino_instance = dino_scene.instance()

	var property = dino_instance.get(prop)

	dino_instance.queue_free()
	return property
