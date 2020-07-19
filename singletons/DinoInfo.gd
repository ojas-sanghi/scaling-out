extends Node

var dino_list := [
	preload("res://actors/dinos/MegaDino.tscn"),
	preload("res://actors/dinos/TankyDino.tscn"),
	preload("res://actors/dinos/WarriorDino.tscn"),
]

var upgrades_info := {
	"mega":
	{
		"hp": [[100, 135, 150], 1],
		"delay": [[2, 1.75, 1.5], 1],
		"def": [[1, 0.95, 0.9, 0.8, 0.75], 1],
		"dodge": [[0], 0],
		"dmg": [[2, 3.5, 5, 6, 7.5, 8, 9, 10, 10.5, 11], 1],
		"speed": [[50, 55, 60, 66, 70, 77, 84, 88], 1],
		"special": [[""], 0]
	},
	"tanky":
	{
		"hp": [[200, 230, 250], 1],
		"delay": [[2, 1.75, 1.5], 1],
		"def": [[1, 0.95, 0.9, 0.8, 0.75], 1],
		"dodge": [[0], 0],
		"dmg": [[2, 3.5, 5, 6, 7.5, 8, 9, 10, 10.5, 11], 1],
		"speed": [[50, 55, 60, 66, 70, 77, 84, 88], 1],
		"ice": [[""], 1]
	},
	"warrior":
	{
		"hp": [[45, 50, 60], 1],
		"delay": [[2, 1.75, 1.5], 1],
		"def": [[1, 0.95, 0.9, 0.8, 0.75], 1],
		"dodge": [[0], 0],
		"dmg": [[2, 3.5, 5, 6, 7.5, 8, 9, 10, 10.5, 11], 1],
		"speed": [[50, 55, 60, 66, 70, 77, 84, 88], 1],
		"fire": [[""], 0]
	}
}


func has_special(dino: String, upgrade: String):
	return upgrades_info[dino][upgrade][1] - 1 == 0


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
