tool
extends Button

export (
	String,
	"EXCEPTION",
	"quit",
	"play",
	"retry combat",
	"retry stealth",
	"ice",
	"fire",
	"return home screen",
	"return upgrades",
	"plus dino",
	"minus dino",
	"buy dinos",
	"continue conquest"
) var button_mode = "EXCEPTION"


func _ready() -> void:
	assert(button_mode != "EXCEPTION")  # error out if no button mode has been set

	set_button_text()


# Sets the text of the button according to what mode it is set to
func set_button_text() -> void:
	match button_mode:
		"quit":
			text = "Quit"
		"play":
			text = "Play"
			self.grab_focus()
		"retry combat":
			text = "Retry"
			self.grab_focus()
		"retry stealth":
			text = "Retry"
			self.grab_focus()
		"return home screen":
			text = "Return Home"
			self.grab_focus()
		"return upgrades":
			text = "Return to Upgrades"
			self.grab_focus()
		"ice":
			text = "Ice Stealth"
		"fire":
			text = "Fire Stealth"
		"plus dino":
			text = "+"
		"minus dino":
			text = "-"
		"buy dinos":
			text = "Purchase"
		"continue conquest":
			text = "Continue Conquest"


func _on_Button_pressed() -> void:
	match button_mode:
		"quit":
			get_tree().quit()
		"play":
			SceneChanger.go_to_scene("res://src/GUI/screens/HomeScreen.tscn")
		"retry combat":
			CombatInfo.reset()
			SceneChanger.go_to_scene("res://src/combat/CombatScreen.tscn")
		"retry stealth":
			if ShopInfo.finding_ice:
				SceneChanger.go_to_scene("res://src/stealth/StealthIce.tscn")
			else:
				SceneChanger.go_to_scene("res://src/stealth/StealthFire.tscn")
		"return home screen":
			SceneChanger.go_to_scene("res://src/GUI/screens/HomeScreen.tscn")
		"return upgrades":
			SceneChanger.go_to_scene("res://src/GUI/screens/UpgradeScreen.tscn")
		"ice":
			SceneChanger.go_to_scene("res://src/stealth/StealthIce.tscn")
		"fire":
			SceneChanger.go_to_scene("res://src/stealth/StealthFire.tscn")
		# nothing for plus/minus: that is handled in that scene
		# same for buy dinos
		"continue conquest":
			Signals.emit_signal("new_round")


func _process(delta: float) -> void:
	if Engine.editor_hint:
		if text == "example text":
			set_button_text()
