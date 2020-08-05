extends RichTextLabel

export(NodePath) var parent_node

var button_mode

var money_cost
var gene_cost

func _ready() -> void:
	Signals.connect("dino_upgraded", self, "set_upgrade_cost")

func _process(delta: float) -> void:
	if get_node(parent_node).info_set:
		button_mode = get_node(parent_node).button_mode
		set_upgrade_cost()
	set_process(false)

func set_upgrade_cost():
	if DinoInfo.get_num_upgrade(ShopInfo.shop_dino, button_mode) == DinoInfo.get_max_upgrade(ShopInfo.shop_dino, button_mode):
		hide()

	var cost = DinoInfo.get_next_upgrade_cost(ShopInfo.shop_dino, button_mode)
	money_cost = cost[0]
	gene_cost = cost[1]


	var money_pic = "[img=<40>]res://assets/icons/coins.png[/img]"
	var gene_pic = "[img=<25>]res://assets/icons/dna.png[/img]"

	var money_text = str(money_cost)
	var gene_text = str(gene_cost)

	if money_cost > ShopInfo.money:
		money_text = "[color=#ff0000]%s[/color]" % money_text
	if gene_cost > ShopInfo.genes:
		gene_text = "[color=#ff0000]%s[/color]" % gene_text

	bbcode_text = "%s%s  %s%s" % [money_pic, money_text, gene_pic, gene_text]
