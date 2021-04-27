extends RichTextLabel

export(NodePath) var parent_node

var button_mode

var gold_cost
var gene_cost

func _ready() -> void:
	Signals.connect("dino_upgraded", self, "set_upgrade_cost")

func _process(delta: float) -> void:
	if get_node(parent_node).info_set:
		button_mode = get_node(parent_node).button_mode
		set_upgrade_cost()
	set_process(false)

func set_upgrade_cost():
	var dino_info = DinoInfo.get_dino(ShopInfo.shop_dino)

	if dino_info.is_maxed_out(button_mode):
		hide()

	var cost = dino_info.get_next_upgrade_cost(button_mode)
	gold_cost = cost[0]
	gene_cost = cost[1]


	var gold_pic = "[img=<40>]res://assets/icons/coins.png[/img]"
	var gene_pic = "[img=<25>]res://assets/icons/dna.png[/img]"

	var gold_text = str(gold_cost)
	var gene_text = str(gene_cost)

	if gold_cost > ShopInfo.gold:
		gold_text = "[color=#ff0000]%s[/color]" % gold_text
	if gene_cost > ShopInfo.genes:
		gene_text = "[color=#ff0000]%s[/color]" % gene_text

	bbcode_text = "%s%s  %s%s" % [gold_pic, gold_text, gene_pic, gene_text]
