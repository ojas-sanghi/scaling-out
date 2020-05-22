extends Node

var dino_id := 0
var dino_list := [
	preload("res://actors/dinos/MegaDino.tscn"),
	preload("res://actors/dinos/TankyDino.tscn"),
	preload("res://actors/dinos/WarriorDino.tscn"),
]
var dinos_deploying := []
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

func _on_dino_deployed():
	dinos_deploying.append(dino_id)

	var delay = get_dino_timer_delay()
	var dinos_deploying_timer = Timer.new()
	dinos_deploying_timer.one_shot = true
	add_child(dinos_deploying_timer)

	dinos_deploying_timer.connect("timeout", self, "_on_dinos_deploying_timer_timeout", [dino_id])

	dinos_deploying_timer.start(delay)

func _on_dinos_deploying_timer_timeout(id):
	dinos_deploying.remove(dinos_deploying.find(id))

# find the delay based on the dino
func get_dino_timer_delay():
	var dino_scene = dino_list[dino_id]
	var dino_instance = dino_scene.instance()
	var delay = dino_instance.deploy_delay
	dino_instance.queue_free()
	return delay

func reset_deploys():
	dinos_deploying = []
	timer_list = []
