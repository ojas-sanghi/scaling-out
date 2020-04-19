extends Button

signal monster_deployed

var spawn_point = Vector2(50, 150)
# TODO: change what we load based on the current active id
var monster1 = preload("res://actors/MegaLizard.tscn")
var monster2 = preload("res://actors/TankyLizard.tscn")
var monster3 = preload("res://actors/WarriorLizard.tscn")

func _on_Lane_pressed() -> void:
	var monster_node

	if Globals.monsters_remaining <= 0:
		return
	if not Globals.can_deploy:
		return

	match Globals.monster_id:
		1:
			monster_node = monster1.instance()
		2:
			monster_node = monster2.instance()
		3:
			monster_node = monster3.instance()

	add_child(monster_node)
	monster_node.position = spawn_point
	emit_signal("monster_deployed")
