extends Path2D

var spawn_point = Vector2(70, 10)

func _on_Button_pressed() -> void:
	if CombatInfo.dinos_remaining <= 0:
		return

	# don't deploy if the delay isn't over yet
	if CombatInfo.dino_id in CombatInfo.dinos_deploying:
		return

	# make a new pathfollow2d node
	var path_follow = PathFollow2D.new()
	path_follow.loop = false
	path_follow.lookahead = 250
	add_child(path_follow)

	# make a new dinosaur
	var dino_node = DinoInfo.dino_list[CombatInfo.dino_id]
	dino_node = dino_node.instance()

	# set the dino speed
	dino_node.path_follow_time = curve.get_baked_length() / dino_node.dino_speed.x

	# add dino to pathfollow2d,
	path_follow.add_child(dino_node)
	# set dino's position
	dino_node.position = spawn_point
	Signals.emit_signal("dino_deployed")
