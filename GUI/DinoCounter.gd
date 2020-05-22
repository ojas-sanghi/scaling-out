extends Control

func _ready() -> void:
	var lanes = get_tree().get_nodes_in_group("lanes")
	if lanes:
		for lane in lanes:
			lane.connect("dino_deployed", self, "_on_dino_deployed")
	update_text()

func _on_dino_deployed():
#	Globals.dinos_remaining -= 1
	update_text()

func update_text():
	pass
#	var label_text = "Remaining dinos: " + str(Globals.dinos_remaining)
#	$Label.text = label_text
