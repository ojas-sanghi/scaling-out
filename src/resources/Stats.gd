extends Resource

export var stat_name = ""
export(Array, float) var stats
export(Resource) var cost

export var level = 0

func get_stat():
	return stats[level]

# pass in level, which defaults to current level
# parameter exists for the get_next_upgrade_cost function
func get_gold(lvl = level):
	return cost.gold[lvl]
func get_genes(lvl = level):
	return cost.genes[lvl]
