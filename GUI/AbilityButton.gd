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

	check_bought_status()

func check_bought_status():
	if dino_option:
		check_dino_specials()
		return
	if DinoInfo.has_upgrade_dino_independent(button_mode):
		set_button_text()
		self.disabled = true
		return
	check_dino_specials()

func check_dino_specials():
	match ShopInfo.shop_dino:
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
				if DinoInfo.has_upgrade_dino_independent("ice"):
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
				if DinoInfo.has_upgrade_dino_independent("fire"):
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
		"speed":
			text = "Speed"
		"fire":
			text = "Fire Projectiles"
		"ice":
			text = "Ice Projectiles"
		"mega":
			text = "Mega"
		"tank":
			text = "Tank"
		"warrior":
			text = "Warrior"
	set_disabled_status()

func set_disabled_status():
	if Engine.editor_hint:
		return
	match button_mode:
		"health":
			if DinoInfo.has_upgrade_dino_independent("health"):
				self.disabled = true
		"speed":
			if DinoInfo.has_upgrade_dino_independent("speed"):
				self.disabled = true
		"fire":
			if DinoInfo.has_upgrade_dino_independent("ice"):
				self.disabled = true
		"ice":
			if DinoInfo.has_upgrade_dino_independent("ice"):
				self.disabled = true
		"mega":
			$Label.hide()
			self.grab_focus()
			self.grab_click_focus()
		"tank":
			$Label.hide()
			var p_node = get_node("../../HBoxContainer/VBoxContainer")
			if p_node != null:
				if DinoInfo.has_upgrade("tanky", "ice"):
					p_node.get_child(1).disabled = true
		"warrior":
			text = "Warrior"
			$Label.hide()
			var p_node = get_node("../../HBoxContainer/VBoxContainer2")
			if p_node != null:
				if DinoInfo.has_upgrade("warrior", "fire"):
					p_node.get_child(1).disabled = true

func _on_Button_pressed() -> void:
	var bought_purchase = false
	if ShopInfo.coins >= cost and not dino_option:
		ShopInfo.coins -= cost
		self.disabled = true
		get_node("/root/UpgradeScreen/CanvasLayer/CoinCounter").update_coin_from_global()
		bought_purchase = true
	match button_mode:
		"health":
			if bought_purchase:
				DinoInfo.add_upgrade("mega", "health")
				DinoInfo.add_upgrade("tanky", "health")
				DinoInfo.add_upgrade("warrior", "health")
		"speed":
			if bought_purchase:
				DinoInfo.add_upgrade("mega", "speed")
				DinoInfo.add_upgrade("tanky", "speed")
				DinoInfo.add_upgrade("warrior", "speed")
		"fire":
			ShopInfo.finding_fire = true
			SceneChanger.go_to_scene("res://stealth/StealthFire.tscn")
		"ice":
			ShopInfo.finding_ice = true
			SceneChanger.go_to_scene("res://stealth/StealthIce.tscn")
		"mega":
			ShopInfo.shop_dino = "mega"
			check_dino_specials()
		"tank":
			ShopInfo.shop_dino = "tank"
			check_dino_specials()
		"warrior":
			ShopInfo.shop_dino = "warrior"
			check_dino_specials()

func _process(delta: float) -> void:
	if Engine.editor_hint:
		if text == "example text":
			set_button_text()
