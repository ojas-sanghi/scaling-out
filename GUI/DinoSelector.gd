extends Node2D

var active_id := 0

func _ready() -> void:
	DinoInfo.connect("dino_deployed", self, "_on_dino_deployed")

	$'1'.show_particles()

func disable_all_other_particles(except: int):
	for n in get_children():
		if n.name != str(except):
			n.hide_particles()

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("dino_1"):
		active_id = 0
		$'1'.show_particles()
		disable_all_other_particles(1)

	elif event.is_action_pressed("dino_2"):
		active_id = 1
		$'2'.show_particles()
		disable_all_other_particles(2)

	elif event.is_action_pressed("dino_3"):
		active_id = 2
		$'3'.show_particles()
		disable_all_other_particles(3)

	elif event.is_action_pressed("dino_4"):
		if not DinoInfo.has_upgrade("tanky", "ice") or CombatInfo.shot_ice:
			return

		for d in get_tree().get_nodes_in_group("dinos"):
			if d.dino_name == "tanky":
				d.shoot_projectile()
				$'4'.disable_ability()
				CombatInfo.shot_ice = true
				return

	elif event.is_action_pressed("dino_5"):
		if not DinoInfo.has_upgrade("warrior", "fire") or CombatInfo.shot_fire:
			return

		for d in get_tree().get_nodes_in_group("dinos"):
			if d.dino_name == "warrior":
				d.shoot_projectile()
				$'5'.disable_ability()
				CombatInfo.shot_fire = true
				return

	DinoInfo.dino_id = active_id

func _on_dino_deployed():
	var dino_name = DinoInfo.get_dino_property("dino_name")

	if dino_name == "tanky":
		if DinoInfo.has_upgrade(dino_name, "ice") and not CombatInfo.shot_ice:
			$'4'.enable_ability()

	if dino_name == "warrior":
		if DinoInfo.has_upgrade(dino_name, "fire") and not CombatInfo.shot_fire:
			$'5'.enable_ability()
