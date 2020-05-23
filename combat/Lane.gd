extends Button

signal dino_deployed

var spawn_point = Vector2(50, 150)

func _ready() -> void:
	connect("dino_deployed", DinoInfo, "_on_dino_deployed")

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

	emit_signal("dino_deployed")
