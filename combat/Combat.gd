extends Node

export var max_dinos = 10

var reward_money = 100

func _ready() -> void:
	CombatInfo.reset(max_dinos)

	Signals.connect("conquest_lost", self, "_on_conquest_lost")
	Signals.connect("conquest_won", self, "_on_conquest_won")


func _on_conquest_lost():
	SceneChanger.go_to_scene("res://GUI/dialogues/CombatLoseDialogue.tscn")

func _on_conquest_won():
	SceneChanger.go_to_scene("res://GUI/dialogues/CombatWinDialogue.tscn")
	ShopInfo.money += reward_money
