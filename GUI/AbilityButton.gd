tool
extends Button

export(String, "EXCEPTION", "health", "speed", "fire", "ice", "mega", "tank", "warrior") var button_mode = "EXCEPTION"
export var cost := 1
export var dino_option = false
export var locked = false

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

#	$Lock.visible = false
#	if locked:
#		$Lock.visible = true
#		self.disabled = true
	check_bought_status()

func check_bought_status():
	if dino_option:
		check_monster_specials()
		return
	if Globals.upgrades[button_mode]:
		set_button_text()
		self.disabled = true
		return
	check_monster_specials()

func check_monster_specials():
	match Globals.shop_monster:
		"mega":
			var p_node = get_node("../../HBoxContainer/VBoxContainer")
			if p_node != null:
				p_node.get_child(1).disabled = true
			var p_node2 = get_node("../../HBoxContainer/VBoxContainer2")
			if p_node2 != null:
				p_node2.get_child(1).disabled = true
		"tank":
			var p_node = get_node("../../HBoxContainer/VBoxContainer")
			if p_node != null:
				if Globals.upgrades["ice"]:
					p_node.get_child(1).disabled = true
					set_button_text()
				p_node.get_child(1).disabled = false
				set_button_text()
			var p_node2 = get_node("../../HBoxContainer/VBoxContainer2")
			if p_node2 != null:
				p_node2.get_child(1).disabled = true
		"warrior":
			var p_node = get_node("../../HBoxContainer/VBoxContainer")
			if p_node != null:
				p_node.get_child(1).disabled = true
				set_button_text()
			var p_node2 = get_node("../../HBoxContainer/VBoxContainer2")
			if p_node2 != null:
				if Globals.upgrades["fire"]:
					p_node2.get_child(1).disabled = true
					set_button_text()
				p_node2.get_child(1).disabled = false
	set_button_text()

# Sets the text of the button according to what mode it is set to
func set_button_text() -> void:
	$Label.text = "Cost: " + str(cost)
	match button_mode:
		"health":
			text = "Health"
			if Globals.upgrades["health"]:
				self.disabled = true
		"speed":
			text = "Speed"
			if Globals.upgrades["speed"]:
				self.disabled = true
		"fire":
			text = "Fire Projectiles"
			if Globals.upgrades["ice"]:
				self.disabled = true
		"ice":
			text = "Ice Projectiles"
			if Globals.upgrades["ice"]:
				self.disabled = true
		"mega":
			text = "Mega"
			$Label.hide()
			self.grab_focus()
			self.grab_click_focus()
		"tank":
			text = "Tank"
			$Label.hide()
			var p_node = get_node("../../HBoxContainer/VBoxContainer")
			if p_node != null:
				if Globals.upgrades["ice"]:
					p_node.get_child(1).disabled = true
		"warrior":
			text = "Warrior"
			$Label.hide()
			var p_node = get_node("../../HBoxContainer/VBoxContainer2")
			if p_node != null:
				if Globals.upgrades["fire"]:
					p_node.get_child(1).disabled = true
	set_disabled_status()

func set_disabled_status():
	if Engine.editor_hint:
		return
	match button_mode:
		"health":
			if Globals.upgrades["health"]:
				self.disabled = true
		"speed":
			if Globals.upgrades["speed"]:
				self.disabled = true
		"fire":
			if Globals.upgrades["ice"]:
				self.disabled = true
		"ice":
			if Globals.upgrades["ice"]:
				self.disabled = true
		"mega":
			$Label.hide()
			self.grab_focus()
			self.grab_click_focus()
		"tank":
			$Label.hide()
			var p_node = get_node("../../HBoxContainer/VBoxContainer")
			if p_node != null:
				if Globals.upgrades["ice"]:
					p_node.get_child(1).disabled = true
		"warrior":
			text = "Warrior"
			$Label.hide()
			var p_node = get_node("../../HBoxContainer/VBoxContainer2")
			if p_node != null:
				if Globals.upgrades["fire"]:
					p_node.get_child(1).disabled = true

func _on_Button_pressed() -> void:
	var bought_purchase = false
	if Globals.coins >= cost and not dino_option:
		Globals.coins -= cost
		self.disabled = true
		get_node("/root/UpgradeScreen/CanvasLayer/CoinCounter").update_coin_from_global()
		bought_purchase = true
	match button_mode:
		"health":
			if bought_purchase:
				Globals.upgrades["health"] = true
		"speed":
			if bought_purchase:
				Globals.upgrades["speed"] = true
		"fire":
			Globals.finding_fire = true
			SceneChanger.go_to_scene("res://stealth/StealthFire.tscn")
		"ice":
			Globals.finding_ice = true
			SceneChanger.go_to_scene("res://stealth/StealthIce.tscn")
		"mega":
			Globals.shop_monster = "mega"
			check_monster_specials()
		"tank":
			Globals.shop_monster = "tank"
			check_monster_specials()
		"warrior":
			Globals.shop_monster = "warrior"
			check_monster_specials()

func _process(delta: float) -> void:
	if Engine.editor_hint:
		if text == "example text":
			set_button_text()
