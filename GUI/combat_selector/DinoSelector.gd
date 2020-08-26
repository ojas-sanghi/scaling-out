extends Node2D

var active_id := 0

var current_num = 1
var selector_sprite = preload("res://GUI/combat_selector/SelectorSprite.tscn")

var selector_list

func _ready() -> void:
	Signals.connect("dino_fully_spawned", self, "_on_dino_fully_spawned")
	Signals.connect("dino_died", self, "_on_dino_died")

	setup_selectors()
	# get a list of children
	selector_list = $HBoxContainer.get_children()

func setup_selectors():
	for icon in DinoInfo.dino_icons:
		var new_selector = selector_sprite.instance()
		new_selector.sprite = icon
		new_selector.text = str(current_num)

		$HBoxContainer.add_child(new_selector)
		current_num += 1

	for ability in DinoInfo.dino_ability_icons:
		if typeof(ability) == TYPE_STRING:
			continue

		# find the filename of the image, which is also the name of the ability itself
		var file_name = ability.resource_path
		var ability_start = file_name.find_last("/")
		var ability_end = file_name.find(".png")
		var ability_string = file_name.substr(ability_start, ability_end)

		var new_selector = selector_sprite.instance()
		new_selector.sprite = ability
		new_selector.text = str(current_num)
		new_selector.ability_mode = ability_string

		new_selector.custom_scale = Vector2(0.07, 0.07)

		$HBoxContainer.add_child(new_selector)
		current_num += 1

	$HBoxContainer.get_children()[0].show_particles()

# turns on particls for this selector and turns off all other particles
func enable_exclusive_particles(number: int):
	var selectors = $HBoxContainer.get_children()
	for s in selectors:
		s.hide_particles()
	selectors[number].show_particles()

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("dino_1"):
		active_id = 0
		enable_exclusive_particles(0)

	elif event.is_action_pressed("dino_2"):
		active_id = 1
		enable_exclusive_particles(1)

	elif event.is_action_pressed("dino_3"):
		active_id = 2
		enable_exclusive_particles(2)

	elif event.is_action_pressed("dino_4"):
		active_id = 3
		enable_exclusive_particles(3)

	elif event.is_action_pressed("dino_5"):
		if not DinoInfo.has_special("tanky") or CombatInfo.shot_ice:
			return

		for d in get_tree().get_nodes_in_group("dinos"):
			if d.dino_name == "tanky":
				d.shoot_projectile()
				selector_list[4].disable_ability()
				CombatInfo.shot_ice = true
				return

	elif event.is_action_pressed("dino_6"):
		if not DinoInfo.has_special("warrior") or CombatInfo.shot_fire:
			return

		for d in get_tree().get_nodes_in_group("dinos"):
			if d.dino_name == "warrior":
				d.shoot_projectile()
				selector_list[5].disable_ability()
				CombatInfo.shot_fire = true
				return

	CombatInfo.dino_id = active_id


func _on_dino_fully_spawned():
	var dino_name = DinoInfo.get_dino_property("dino_name")

	if dino_name == "tanky":
		if DinoInfo.has_special(dino_name) and not CombatInfo.shot_ice:
			selector_list[4].enable_ability()

	if dino_name == "warrior":
		if DinoInfo.has_special(dino_name) and not CombatInfo.shot_fire:
			selector_list[5].enable_ability()


func _on_dino_died(type):
	var dinos_left = get_tree().get_nodes_in_group("dinos")
	if type == "tanky":
		for dino in dinos_left:
			if dino.dino_name == "tanky":
				return
		selector_list[4].disable_ability()

	if type == "warrior":
		for dino in dinos_left:
			if dino.dino_name == "warrior":
				return
		selector_list[5].disable_ability()
