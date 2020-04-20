extends Button

export(String, "EXCEPTION", "quit", "play", "retry level", "return home screen", "return upgrades") var button_mode = "EXCEPTION"

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
		print(root_node.name)
		print(root_node.filename)

		assert(button_mode != "EXCEPTION") # error out if no button mode has been set

	set_button_text()

# Sets the text of the button according to what mode it is set to
func set_button_text() -> void:
	match button_mode:
		"quit":
			text = "Quit"
		"play":
			text = "Play"
			self.grab_focus()
		"retry level":
			text = "Retry"
			self.grab_focus()
		"return home screen":
			text = "Return Home"
			self.grab_focus()
		"return upgrades":
			text = "Return to Upgrades"
			self.grab_focus()

func _on_Button_pressed() -> void:
	match button_mode:
		"quit":
			get_tree().quit()
		"play":
			SceneChanger.go_to_scene("res://GUI/Screens/HomeScreen.tscn")
		"retry level":
			var timers = get_tree().get_nodes_in_group("level_timer")
			if timers:
				timers[0].reset_time()
			Globals.monsters_remaining = Globals.max_monsters
			get_tree().reload_current_scene()
		"return home screen":
			get_tree().change_scene("res://GUI/Screens/HomeScreen.tscn")
		"return upgrades":
			SceneChanger.go_to_scene("res://GUI/Screens/UpgradeScreen.tscn")
