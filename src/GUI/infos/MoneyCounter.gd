extends Control

onready var money_amt = ShopInfo.money


func _ready() -> void:
	Signals.connect("coin_grabbed", self, "_on_coin_grabbed")
	Signals.connect("dino_upgraded", self, "update_coin_from_global")

	update_coin_from_global()


func _on_money_grabbed(value):
	money_amt += value
	update_money_amt()


func update_money_amt():
	$Num.text = str(money_amt)


func update_coin_from_global():
	$Num.text = str(ShopInfo.money)
