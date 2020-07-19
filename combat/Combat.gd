extends Node

export var max_dinos = 10


func _ready() -> void:
	CombatInfo.reset(max_dinos)

	Signals.connect("game_over", self, "_on_game_over")


func _on_game_over():
	SceneChanger.go_to_scene("res://GUI/CombatLoseDialogue.tscn")
