extends Area2D

func _on_Blockade_area_entered(area: Area2D) -> void:
	if "Fire" in area.name:
		Signals.emit_signal("proj_hit", "fire")
	elif "Ice" in area.name:
		Signals.emit_signal("proj_hit", "ice")
