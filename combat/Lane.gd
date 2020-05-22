extends Button

signal monster_deployed

var spawn_point = Vector2(50, 150)

func _ready() -> void:
	connect("monster_deployed", DinoInfo, "_on_monster_deployed")

func _on_Lane_pressed() -> void:
#	if Globals.monsters_remaining <= 0:
#		return

	if DinoInfo.monster_id in DinoInfo.deploying:
		return

	var monster_node = DinoInfo.monster_list[DinoInfo.monster_id]
	monster_node = monster_node.instance()

	add_child(monster_node)
	monster_node.position = spawn_point

	emit_signal("monster_deployed")
