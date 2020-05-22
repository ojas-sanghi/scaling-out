extends Node

export var max_dinos = 10

func _ready() -> void:
	CombatInfo.max_dinos = max_dinos
	CombatInfo.dinos_remaining = max_dinos

	DinoInfo.reset_deploys()
