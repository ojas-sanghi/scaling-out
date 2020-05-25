extends Control

func _ready() -> void:
	Signals.connect("dino_deployed", self, "_on_dino_deployed")
	update_text()

func _on_dino_deployed():
#	ShopInfo.dinos_remaining -= 1
	update_text()

func update_text():
	pass
#	var label_text = "Remaining dinos: " + str(ShopInfo.dinos_remaining)
#	$Label.text = label_text
