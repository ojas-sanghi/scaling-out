extends Node

signal dino_deployed
signal dino_fully_spawned
signal dino_died

signal dino_hit

signal game_over

signal coin_grabbed

signal level_passed
signal level_failed

signal proj_hit

signal all_dinos_expended


func _ready() -> void:
	OS.window_maximized = true
