extends Node

export var max_dinos = 10
export var reward_gold = 100

export var max_rounds = 3

func _ready() -> void:
	CombatInfo.reset(max_dinos, max_rounds)
	# can't put that in reset() since otherwise it would do that between rounds
	# note: this executes AFTER RoundCounter grabs the data, so...
	CombatInfo.current_round = 3

	#4: one-shot round won connection
	Signals.connect("round_won", self, "_on_round_won", [], 4)
	Signals.connect("new_round", self, "_on_new_round")

	Signals.connect("conquest_lost", self, "_on_conquest_lost")
	Signals.connect("conquest_won", self, "_on_conquest_won")
	Signals.connect("dinos_purchased", self, "_on_dinos_purchased")

func _on_round_won():
	CombatInfo.current_round += 1

	CombatInfo.creds += 150
	CombatInfo.creds += 50 * CombatInfo.num_army_elim

	# Num of max dinos for next round is remaining + number of bought
	CombatInfo.max_dinos = CombatInfo.dinos_remaining

	if CombatInfo.current_round > CombatInfo.max_rounds:
		Signals.emit_signal("conquest_won")
		return

	get_tree().paused = true
	$BuyMenu.show()

# when a new rounds starts, re-connect the won round signal
func _on_new_round():
	Signals.connect("round_won", self, "_on_round_won", [], 4)

func _on_conquest_lost():
	SceneChanger.go_to_scene("res://src/GUI/dialogues/CombatLoseDialogue.tscn")

func _on_conquest_won():
	SceneChanger.go_to_scene("res://src/GUI/dialogues/CombatWinDialogue.tscn")
	ShopInfo.gold += reward_gold

func _on_dinos_purchased(num):
	max_dinos += num
	CombatInfo.creds -= num * DinoInfo.dino_cred_cost
	CombatInfo.reset(max_dinos)
