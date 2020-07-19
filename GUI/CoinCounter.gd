extends Control

onready var coin_amt = ShopInfo.coins


func _ready() -> void:
	var coins = get_tree().get_nodes_in_group("coins")
	Signals.connect("coin_grabbed", self, "_on_coin_grabbed")
	update_coin_amt()


func _on_coin_grabbed(value):
	coin_amt += value
	update_coin_amt()


func update_coin_amt():
	$Num.text = str(coin_amt)


func update_coin_from_global():
	$Num.text = str(ShopInfo.coins)
