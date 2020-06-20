extends GeneralDino

var dino_name = "mega"
var speed := Vector2(50, 0)
var health = 100
var dmg = 5
var cost = 0
var gene = ""

var variations = ["blue", "green", "orange"]

func _init():
	if DinoInfo.has_upgrade(dino_name, "speed"):
		speed.x += 10
	if DinoInfo.has_upgrade(dino_name, "health"):
		health += 25
	if DinoInfo.has_upgrade(dino_name, "dmg"):
		dmg += 3

	._init(dino_name, speed, health, variations, dmg, cost, gene)
