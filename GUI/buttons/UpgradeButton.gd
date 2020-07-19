tool
class_name UpgradeButton extends Button

export (String, "EXCEPTION", "dmg", "def", "speed", "ice", "fire", "hp", "delay") var button_mode = "EXCEPTION"

onready var max_squares = DinoInfo.get_max_upgrade(ShopInfo.shop_dino, button_mode)

onready var container_length = $Container/Squares.rect_size.x
onready var container_height = $Container/Squares.rect_size.y


func _ready() -> void:
	assert(button_mode != "EXCEPTION")

	do_everything()


func set_stat_info():
	pass


func set_button_info():
	text = ""
	$Container/StatNum.text = str(DinoInfo.get_upgrade_stat(ShopInfo.shop_dino, button_mode))

	match button_mode:
		"dmg":
			$Name.text = "Damage"
			$Container/Stat.text = "DPS"
			$Img.texture = preload("res://assets/abilities/fire.png")

		"def":
			$Name.text = "Defense"
			$Container/Stat.text = "%"
			$Img.texture = preload("res://assets/abilities/health.png")

		"speed":
			$Name.text = "Leg Speed"
			$Container/Stat.text = "KM/h"
			$Img.texture = preload("res://assets/abilities/speed.png")

		"ice":
			$Name.text = "Special"
			$Container/StatNum.text = ""
			$Container/Stat.text = ""
			$Img.texture = preload("res://assets/abilities/ice.png")

		"fire":
			$Name.text = "Special"
			$Container/StatNum.text = ""
			$Container/Stat.text = ""
			$Img.texture = preload("res://assets/abilities/fire.png")


func color_squares(color = Color(1, 1, 1)):
	# fill in the squares
	var squares_list = $Container/Squares.get_children()
	var filled_squares = DinoInfo.get_num_upgrade(ShopInfo.shop_dino, button_mode)

	match button_mode:
		"hp":
			color = Color("e61a1a")
		"delay":
			color = Color("1a82e6")

	for j in filled_squares:
		if filled_squares == max_squares:
			squares_list[j].color = Color(0, 1, 0)
		else:
			squares_list[j].color = color


func set_upgrade_squares():
	# make the backgorund squares
	for i in max_squares:
		var new_square = ColorRect.new()
		new_square.color = Color(0, 0, 0, 0.5)
		new_square.rect_min_size = Vector2(container_length / max_squares, container_height)
		new_square.rect_size = new_square.rect_min_size

		$Container/Squares.add_child(new_square)

	color_squares()


func do_everything():
	set_button_info()
	set_upgrade_squares()


###########


func stop_upgrading():
	$Tween.set_active(false)
	$Tween.reset_all()


func _on_UpgradeButton_button_down() -> void:
	# don't do anything if max upgrades reached
	if DinoInfo.check_max_upgrade(ShopInfo.shop_dino, button_mode):
		return

	$Tween.interpolate_property($TextureProgress, "value", 0, 100, 1.5)
	$Tween.start()


func _on_UpgradeButton_button_up() -> void:
	stop_upgrading()


func _on_Tween_tween_completed(object: Object, key: NodePath) -> void:
	DinoInfo.add_upgrade(ShopInfo.shop_dino, button_mode)

	stop_upgrading()
	color_squares()

	$Container/StatNum.text = str(DinoInfo.get_upgrade_stat(ShopInfo.shop_dino, button_mode))
