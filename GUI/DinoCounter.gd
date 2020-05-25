extends Control

func _ready() -> void:
	Signals.connect("dino_deployed", self, "_on_dino_deployed")
	update_text()

func _on_dino_deployed():
	CombatInfo.dinos_remaining -= 1
	if CombatInfo.dinos_remaining == 0:
		Signals.emit_signal("all_dinos_expended")
	update_text()

func update_text():
	var label_text = "Remaining dinos: " + str(CombatInfo.dinos_remaining)
	$Label.text = label_text
