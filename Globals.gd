extends Node

var monster_id = 1
var monster_dict = {
	1: "Mega Lizard",
	2: "Tanky Lizard",
	3: "Warrior Lizard",
}

var upgrades = {
	"health": false,
	"speed": false,
	"fire": false,
	"ice": false,
}

var finding_ice = false
var finding_fire = false

var max_monsters = 7
var monsters_remaining = max_monsters
var monsters_died = 0
var can_deploy = true

var player_caught = false
var coins = 100

var monster_health = 100
var monster_speed := Vector2(50, 0)
var tank_monster_health = 200
var tank_monster_speed := Vector2(35, 0)
var warrior_monster_health = 45
var warrior_monster_speed := Vector2(100, 0)

var shop_monster = "mega"

func try_connect_lanes():
	var lanes = get_tree().get_nodes_in_group("lanes")
	if lanes:
		set_process(false)
		for lane in lanes:
			lane.connect("monster_deployed", self, "_on_monster_deployed")

func _ready() -> void:
	OS.window_maximized = true

	if upgrades["health"]:
		monster_health += 50
	if upgrades["speed"]:
		monster_speed.x += 50

func _process(delta: float) -> void:
	try_connect_lanes()

func _on_monster_deployed():
	can_deploy = false
	yield(get_tree().create_timer(2.0), "timeout")
	can_deploy = true

func get_monster_id():
	return monster_id

func get_monster_name():
	return monster_dict.get(monster_id)
