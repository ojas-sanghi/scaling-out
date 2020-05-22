extends Node

var finding_ice = false
var finding_fire = false

var player_caught = false
var coins = 100

var shop_monster = "mega"

func _ready() -> void:
	OS.window_maximized = true
