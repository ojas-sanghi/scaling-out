extends GeneralDino

var dino_name = "tanky"
var speed := Vector2(35, 0)
var health = 200
var dmg = 2
var cost = 250
var gene = "ice"

var variations = ["blue", "orange", "pink"]


func _init():
	if DinoInfo.has_upgrade(dino_name, "speed"):
		speed.x += 5
	if DinoInfo.has_upgrade(dino_name, "health"):
		health += 75
	if DinoInfo.has_upgrade(dino_name, "dmg"):
		dmg += 3

	._init(dino_name, speed, health, variations, dmg, cost, gene)

func _ready() -> void:
	$IceProjectile.hide()

func shoot_projectile():
	$IceProjectile.show()
	$IceProjectile.disabled = false
