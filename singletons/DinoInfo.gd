extends Node

var dino_cred_cost = 30

var dino_list := [
	preload("res://actors/dinos/MegaDino.tscn"),
	preload("res://actors/dinos/TankyDino.tscn"),
	preload("res://actors/dinos/WarriorDino.tscn"),
	preload("res://actors/dinos/GatorGecko.tscn"),
]

var dino_icons := [
	preload("res://assets/dinos/mega_dino/mega_dino.png"),
	preload("res://assets/dinos/tanky_dino/Armored_Dino_ICON.png"),
	preload("res://assets/dinos/warrior_dino/Tribal_Dino_icon.png"),
	preload("res://assets/dinos/gator_gecko/gater_gecko_icon.png"),
]

var dino_ability_icons = [
	"",
	preload("res://assets/dinos/misc/ice.png"),
	preload("res://assets/dinos/misc/fire.png"),
	""
]

var upgrades_info := {
	"mega":
	{
		"hp": [[100, 135, 150], 1],
		"delay": [[2, 1.75, 1.5], 1],
		"def": [[1, 0.98, 0.95, 0.92, 0.87], 1],
		"dodge": [[0], 0],
		"dmg": [[2, 3.5, 5, 6, 7.5, 8, 9, 10, 10.5, 11], 1],
		"speed": [[50, 55, 60, 66, 70, 77, 84, 88], 1],
		"special": [[""], 0],
	},
	"tanky":
	{
		"hp": [[200, 230, 250], 1],
		"delay": [[2, 1.75, 1.5], 1],
		"def": [[1, 0.9, 0.86, 0.82, 0.75], 1],
		"dodge": [[0], 0],
		"dmg": [[0.5, 0.75, 0.8, 0.9, 1, 1.5, 2, 2.2, 2.5, 2.8], 1],
		"speed": [[30, 35, 40, 42, 45, 50, 56, 60], 1],
		"special": [[""], 1]
	},
	"warrior":
	{
		"hp": [[450, 50, 60], 1],
		"delay": [[0.1, 1.75, 1.5], 1],
		"def": [[1, 0.99, 0.98, 0.95, 0.93], 1],
		"dodge": [[0], 0],
		"dmg": [[500, 5.5, 6, 7, 8, 8.5, 9.2, 9.9, 10.6, 11.5], 1],
		"speed": [[600, 65, 70, 79, 86, 95, 105, 110], 1],
		"special": [[""], 0]
	},
	"gator":
	{
		"hp": [[45, 50, 60], 1],
		"delay": [[2, 1.75, 1.5], 1],
		"def": [[1, 0.99, 0.98, 0.95, 0.93], 1],
		"dodge": [[0], 0],
		"dmg": [[5, 5.5, 6, 7, 8, 8.5, 9.2, 9.9, 10.6, 11.5], 1],
		"speed": [[60, 65, 70, 79, 86, 95, 105, 110], 1],
		"special": [[""], 0]
	},
}

var upgrades_cost = {
	"mega": {
		"hp": [[50, 100, 150], [30, 50, 80]],
		"delay": [[50, 100, 150], [30, 50, 80]],
		"def": [[50, 100, 150], [30, 50, 80]],
		"dodge": [[50, 100, 150], [30, 50, 80]],
		"dmg": [[50, 100, 150], [30, 50, 80]],
		"speed": [[50, 100, 150], [30, 50, 80]],
		"special": [[50, 100, 150], [30, 50, 80]],
	},
	"tanky": {
		"hp": [[50, 100, 150], [30, 50, 80]],
		"delay": [[50, 100, 150], [30, 50, 80]],
		"def": [[50, 100, 150, 175, 200], [30, 50, 80, 110, 140]],
		"dodge": [[50, 100, 150], [30, 50, 80]],
		"dmg": [[50, 100, 150], [30, 50, 80]],
		"speed": [[50, 100, 150], [30, 50, 80]],
		"special": [[50, 100, 150], [30, 50, 80]],
	},
	"warrior": {
		"hp": [[50, 100, 150], [30, 50, 80]],
		"delay": [[50, 100, 150], [30, 50, 80]],
		"def": [[50, 100, 150], [30, 50, 80]],
		"dodge": [[50, 100, 150], [30, 50, 80]],
		"dmg": [[50, 100, 150], [30, 50, 80]],
		"speed": [[50, 100, 150], [30, 50, 80]],
		"special": [[50, 100, 150], [30, 50, 80]],
	},
	"gator": {
		"hp": [[50, 100, 150], [30, 50, 80]],
		"delay": [[50, 100, 150], [30, 50, 80]],
		"def": [[50, 100, 150], [30, 50, 80]],
		"dodge": [[50, 100, 150], [30, 50, 80]],
		"dmg": [[50, 100, 150], [30, 50, 80]],
		"speed": [[50, 100, 150], [30, 50, 80]],
		"special": [[50, 100, 150], [30, 50, 80]],
	},
}

func has_special(dino: String):
	return upgrades_info[dino]["special"][1] - 1 == 0


func add_upgrade(dino: String, upgrade: String) -> void:
	upgrades_info[dino][upgrade][1] += 1


func get_max_upgrade(dino: String, upgrade: String) -> int:
	return upgrades_info[dino][upgrade][0].size()


func get_num_upgrade(dino: String, upgrade: String) -> int:
	return upgrades_info[dino][upgrade][1]


func get_upgrade_stat(dino: String, upgrade: String):
	var num_upgrade = get_num_upgrade(dino, upgrade) - 1
	if num_upgrade < 0:
		num_upgrade += 1
	return upgrades_info[dino][upgrade][0][num_upgrade]

# returns [money, genes]
func get_next_upgrade_cost(dino: String, upgrade: String):
	var current_level = get_num_upgrade(dino, upgrade)

	if current_level == get_max_upgrade(dino, upgrade):
		return [0, 0]

	# we don't need to increment the current_level since it's 0 indexed
	# a level of 1 means that we're at pos. 0 in the list
	# and if we use that number, we'll be returning the cost of pos. 1 in the list; the 2nd position
	# ez
	var next_money = upgrades_cost[dino][upgrade][0][current_level]
	var next_gene = upgrades_cost[dino][upgrade][1][current_level]

	return [next_money, next_gene]

func check_max_upgrade(dino: String, upgrade: String) -> bool:
	return get_num_upgrade(dino, upgrade) == get_max_upgrade(dino, upgrade)


func get_dino_timer_delay():
	# find the delay based on the dino
	return get_dino_property("deploy_delay")


func get_dino_property(prop: String):
	# instance it, get the variable we want, then remove it
	var dino_scene = dino_list[CombatInfo.dino_id]
	var dino_instance = dino_scene.instance()

	var property = dino_instance.get(prop)

	dino_instance.queue_free()
	return property
