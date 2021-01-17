extends Control

func _ready() -> void:
	hide()
#	show()


func _on_BuyMenu_visibility_changed() -> void:
	update_max_value()
	$CredCounter.set_creds()

	if CombatInfo.num_army_elim == 0:
		$Labels/ArmyElimBonus.text = ""
	else:
		$Labels/ArmyElimBonus.text = "Army elimination bonus: +" + str(50 * CombatInfo.num_army_elim)

func update_max_value():
	$Buy/SpinBox.max_value = CombatInfo.creds / DinoInfo.dino_cred_cost

func _on_Minus_pressed() -> void:
	$Buy/SpinBox.value -= 1

func _on_Plus_pressed() -> void:
	$Buy/SpinBox.value += 1


func _on_Purchase_pressed() -> void:
	Signals.emit_signal("dinos_purchased", $Buy/SpinBox.value)

	update_max_value()
	$CredCounter.set_creds()

func _on_Conquest_pressed() -> void:
	hide()
	get_tree().paused = false
