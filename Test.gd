extends Node2D

func _ready() -> void:
	var data = load("res://src/actors/dinos/stats/MegaDino.tres")

	print(data.hpStat.stats)
