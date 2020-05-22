extends Node

var monster_id := 0
var monster_list := [
	preload("res://actors/dinos/MegaLizard.tscn"),
	preload("res://actors/dinos/TankyLizard.tscn"),
	preload("res://actors/dinos/WarriorLizard.tscn"),
]
var deploying := []
var timer_list := []

# {tanky: [health, dmg], mega: [health]}
var upgrades_list := {}

func has_upgrade(dino: String, upgrade: String) -> bool:
	return dino in upgrades_list and upgrade in upgrades_list[dino]

func add_upgrade(dino: String, upgrade: String) -> void:
	if dino in upgrades_list:
		upgrades_list[dino].append(upgrade)
	else:
		upgrades_list[dino] = [upgrade]

func _on_monster_deployed():
	deploying.append(monster_id)

	var delay = get_dino_timer_delay()
	var deploying_timer = Timer.new()
	deploying_timer.one_shot = true
	add_child(deploying_timer)

	deploying_timer.connect("timeout", self, "_on_deploying_timer_timeout", [monster_id])

	deploying_timer.start(delay)

func _on_deploying_timer_timeout(id):
	deploying.remove(deploying.find(id))

# find the delay based on the dino
func get_dino_timer_delay():
	var dino_scene = monster_list[monster_id]
	var dino_instance = dino_scene.instance()
	var delay = dino_instance.deploy_delay
	dino_instance.queue_free()
	return delay

func reset_deploys():
	deploying = []
	timer_list = []
