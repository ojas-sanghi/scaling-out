tool
extends TextureButton

export(String, "EXCEPTION", "map", "upgrades") var button_mode = "EXCEPTION"

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
	match button_mode:
		"map":
			$Label.text = "Go fight!"
		"upgrades":
			$Label.text = "View upgrades menu"

func _on_Button_pressed() -> void:
	match button_mode:
		"map":
			SceneChanger.go_to_scene("res://combat/CombatScreen.tscn")
		"upgrades":
			SceneChanger.go_to_scene("res://GUI/Screens/UpgradeScreen.tscn")

func _process(delta: float) -> void:
	if Engine.editor_hint:
		if $Label.text == "example text":
			set_button_text()