extends Resource

export var stat_name = "Special"
export var special := ""
export(Resource) var cost

export var level = 0

func get_stat():
	return special

func get_gold(lvl = level):
	return cost.gold[lvl]

func get_genes(lvl = level):
	return cost.genes[lvl]
