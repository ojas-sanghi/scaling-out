class_name SpecialStat
extends Resource

export var stat_name = "Special"
export var special := ""
export(Resource) var cost

export var level = 0

func get_stat():
	return special

func get_gold_cost():
	return cost.gold[level]

func get_genes_cost():
	return cost.genes[level]
