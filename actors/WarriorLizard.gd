extends GeneralMonster

func _init().(Globals.warrior_monster_speed, Globals.warrior_monster_health):
	pass

func shoot_projectile():
	$FireProjectile.show()
	$FireProjectile.disabled = false
