extends Node

export var max_dinos = 10
var reward_money = 100

var max_rounds = 3
var current_round = 1

var creds = 100

func _ready() -> void:
	CombatInfo.reset(max_dinos)

	Signals.connect("round_won", self, "_on_round_won")
	Signals.connect("conquest_lost", self, "_on_conquest_lost")
	Signals.connect("conquest_won", self, "_on_conquest_won")

func _on_round_won():
	current_round += 1
	if current_round > max_rounds:
		Signals.emit_signal("conquest_won")

func _on_conquest_lost():
	SceneChanger.go_to_scene("res://GUI/dialogues/CombatLoseDialogue.tscn")

func _on_conquest_won():
	SceneChanger.go_to_scene("res://GUI/dialogues/CombatWinDialogue.tscn")
	ShopInfo.money += reward_money
