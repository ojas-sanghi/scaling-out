extends Node

signal dino_deployed
signal dino_fully_spawned
signal dino_died

signal dino_hit
signal blockade_hit

signal new_round
signal round_won
signal conquest_won
signal conquest_lost

signal dinos_purchased

signal coin_grabbed

signal level_passed
signal level_failed

signal proj_hit

signal all_dinos_expended

signal dino_upgraded


func _ready() -> void:
	OS.window_maximized = true
