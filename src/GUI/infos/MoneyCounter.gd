extends Control

onready var gold_amt = ShopInfo.gold


func _ready() -> void:
	Signals.connect("coin_grabbed", self, "_on_coin_grabbed")
	Signals.connect("dino_upgraded", self, "update_coin_from_global")

	update_coin_from_global()


func _on_coin_grabbed(value):
	gold_amt += value
	update_gold_amt()


func update_gold_amt():
	$Num.text = str(gold_amt)


func update_coin_from_global():
	$Num.text = str(ShopInfo.gold)
