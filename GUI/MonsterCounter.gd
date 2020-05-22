extends Control

func _ready() -> void:
	var lanes = get_tree().get_nodes_in_group("lanes")
	if lanes:
		for lane in lanes:
			lane.connect("monster_deployed", self, "_on_monster_deployed")
	update_text()

func _on_monster_deployed():
#	Globals.monsters_remaining -= 1
	update_text()

func update_text():
	pass
#	var label_text = "Remaining monsters: " + str(Globals.monsters_remaining)
#	$Label.text = label_text
