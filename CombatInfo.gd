extends Node

var max_dinos: int
var dinos_remaining: int
var dinos_died: int

func _ready() -> void:
	reset()

func reset() -> void:
	max_dinos = 10
	dinos_remaining = max_dinos
	dinos_died = 0
