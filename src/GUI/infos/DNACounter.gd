extends Control


func _ready() -> void:
	Signals.connect("dino_upgraded", self, "update_gene_amt")

	update_gene_amt()


func update_gene_amt():
	$Label.text = str(ShopInfo.genes)
