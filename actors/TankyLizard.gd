extends GeneralMonster

func _init().(Globals.tank_monster_speed, Globals.tank_monster_health):
	pass

func shoot_projectile():
	$IceProjectile.show()
	$IceProjectile.disabled = false
