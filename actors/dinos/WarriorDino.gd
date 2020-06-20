extends GeneralDino

var dino_name = "warrior"
var speed = Vector2(100, 0)
var health = 45
var dmg = 10
var cost = 350
var gene = "fire"

var variations = ["green", "purple", "red"]

func _init():
	if DinoInfo.has_upgrade(dino_name, "speed"):
		speed.x += 45
	if DinoInfo.has_upgrade(dino_name, "health"):
		health += 15
	if DinoInfo.has_upgrade(dino_name, "dmg"):
		dmg += 5

	._init(dino_name, speed, health, variations, dmg, cost, gene)

func _ready() -> void:
	$FireProjectile.hide()

func shoot_projectile():
	$FireProjectile.show()
	$FireProjectile.disabled = false
