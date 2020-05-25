extends Node

var dino_list := [
	preload("res://actors/dinos/MegaDino.tscn"),
	preload("res://actors/dinos/TankyDino.tscn"),
	preload("res://actors/dinos/WarriorDino.tscn"),
]

# {tanky: [health, dmg], mega: [health]}
var upgrades_list := {}


func has_upgrade(dino: String, upgrade: String) -> bool:
	return dino in upgrades_list and upgrade in upgrades_list[dino]

# note: likely temporary
# fix to get shop working
# implementation of upgrades will change so this might not be used
# plus new ui from raj
func has_upgrade_dino_independent(upgrade: String) -> bool:
	for dino in upgrades_list:
		return upgrade in upgrades_list[dino]

func add_upgrade(dino: String, upgrade: String) -> void:
	if dino in upgrades_list:
		upgrades_list[dino].append(upgrade)
	else:
		upgrades_list[dino] = [upgrade]


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
