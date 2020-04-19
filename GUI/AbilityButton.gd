extends Button

export(String, "EXCEPTION", "health", "speed", "fire", "ice") var button_mode = "EXCEPTION"
export var cost := 1

func _ready() -> void:
	if button_mode == "EXCEPTION":
		# print out name and location for debugging convenience
		print(name)

		# find number of "../" needed to reach the root node and get node using thjat
		# then print out its name and location
		# we have to do this because we don't know the name of the root node
		var path = ""
		for _i in range(0, get_position_in_parent()):
			path += "../"
		var root_node = get_node(path)
#		print(root_node.name)
#		print(root_node.filename)

		assert(button_mode != "EXCEPTION") # error out if no button mode has been set

	set_button_text()

# Sets the text of the button according to what mode it is set to
func set_button_text() -> void:
	$Label.text = "Cost: " + str(cost)
	match button_mode:
		"health":
			text = "Health"
			self.grab_focus()
		"speed":
			text = "Speed"
		"fire":
			text = "Fire Projectiles"
		"ice":
			text = "Ice Projectiles"

func _on_Button_pressed() -> void:
	match button_mode:
		"quit":
			get_tree().quit()
		"play":
			SceneChanger.go_to_scene("res://stealth/StealthScreen.tscn")
		"home screen":
			SceneChanger.go_to_scene("res://gui/screens/TitleScreen.tscn")
		"retry level":
			var timers = get_tree().get_nodes_in_group("level_timer")
			if timers:
				timers[0].reset_time()
			Globals.monsters_remaining = Globals.max_monsters
			get_tree().reload_current_scene()
		"return main menu":
			pass
