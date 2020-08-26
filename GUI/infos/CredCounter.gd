extends Control

func _ready() -> void:
	set_creds()

func set_creds():
	$HBoxContainer/Label.text = str(CombatInfo.creds)
