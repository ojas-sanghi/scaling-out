extends Node

var monster_id = 1
var monster_dict = {
	1: "Big Lizard",
	2: "Tanky Lizard",
	3: "Bob the Lizard",
}

var max_monsters = 15
var monsters_remaining = max_monsters
var monsters_died = 0
var can_deploy = true

func _ready() -> void:
	OS.window_maximized = true
	var lanes = get_tree().get_nodes_in_group("lanes")
	if lanes:
		for lane in lanes:
			lane.connect("monster_deployed", self, "_on_monster_deployed")

func _on_monster_deployed():
	can_deploy = false
	yield(get_tree().create_timer(2, false), "timeout")
	can_deploy = true

func get_monster_id():
	return monster_id

func get_monster_name():
	return monster_dict.get(monster_id)
