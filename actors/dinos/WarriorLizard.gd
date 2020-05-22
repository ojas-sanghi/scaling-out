extends GeneralMonster

var monster_name = "warrior"
var speed = Vector2(100, 0)
var health = 45
var dmg = 10

var variations = ["green", "purple", "red"]

func _init():
	if DinoInfo.has_upgrade(monster_name, "speed"):
		speed.x += 45
	if DinoInfo.has_upgrade(monster_name, "health"):
		health += 15
	if DinoInfo.has_upgrade(monster_name, "dmg"):
		dmg += 5

	._init(speed, health, variations, dmg)

func _ready() -> void:
	$FireProjectile.hide()

func shoot_projectile():
	$FireProjectile.show()
	$FireProjectile.disabled = false
