tool
extends TextureButton

export (String, "EXCEPTION", "map", "upgrades") var button_mode = "EXCEPTION"


func _ready() -> void:
	assert(button_mode != "EXCEPTION")  # error out if no button mode has been set
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
			SceneChanger.go_to_scene("res://src/combat/CombatScreen.tscn")
		"upgrades":
			SceneChanger.go_to_scene("res://src/GUI/screens/UpgradeScreen.tscn")


func _process(delta: float) -> void:
	if Engine.editor_hint:
		if $Label.text == "example text":
			set_button_text()
