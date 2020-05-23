extends Node

export var max_dinos = 10

func _ready() -> void:
	CombatInfo.max_dinos = max_dinos
	CombatInfo.dinos_remaining = max_dinos

	DinoInfo.reset_deploys()

	var timers = get_tree().get_nodes_in_group("level_timer")
	if timers:
		timers[0].connect("timer_timeout", self, "game_over")

func game_over():
	SceneChanger.go_to_scene("res://GUI/CombatLoseDialogue.tscn")
