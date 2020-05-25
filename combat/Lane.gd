extends Button

var spawn_point = Vector2(50, 150)

func _on_Lane_pressed() -> void:
	if CombatInfo.dinos_remaining <= 0:
		return

	# don't deploy if the delay isn't over yet
	if DinoInfo.dino_id in DinoInfo.dinos_deploying:
		return

	var dino_node = DinoInfo.dino_list[DinoInfo.dino_id]
	dino_node = dino_node.instance()

	add_child(dino_node)
	dino_node.position = spawn_point

	Signals.emit_signal("dino_deployed")
