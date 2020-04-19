extends Button

export(String, "EXCEPTION", "quit", "play", "home screen", "retry level", "return home screen") var button_mode = "EXCEPTION"

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
		"home screen":
			text = "Return to the Home screen"
		"play":
			text = "Play"
			self.grab_focus()
		"retry level":
			text = "Retry"
			self.grab_focus()
		"return home screen":
			text = "Return home"
			self.grab_focus()

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
		"return home screen":
			pass
