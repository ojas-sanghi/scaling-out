tool
class_name UpgradeButton extends Button

export (String, "EXCEPTION", "dmg", "def", "speed", "ice", "fire", "hp", "delay") var button_mode = "EXCEPTION"

var max_squares
var first_run = true

# this is checked by the CostIndicator
# used to wait for the info to be set and for the button_mode to be "special" if needed
# once this is true, the cost indicator retrieves the info needed
var info_set := false

var money_cost: int
var gene_cost: int

onready var container_length = $Container/Squares.rect_size.x
onready var container_height = $Container/Squares.rect_size.y

func _ready() -> void:
	assert(button_mode != "EXCEPTION")

	do_everything()


func set_button_info():
	text = ""

	match button_mode:
		"dmg":
			$NameCostContainer/Name.text = "Damage"
			$Container/Stat.text = "DPS"
			$Img.texture = preload("res://assets/abilities/fire.png")

		"def":
			$NameCostContainer/Name.text = "Defense"
			$Container/Stat.text = "%"
			$Img.texture = preload("res://assets/abilities/health.png")

		"speed":
			$NameCostContainer/Name.text = "Leg Speed"
			$Container/Stat.text = "KM/h"
			$Img.texture = preload("res://assets/abilities/speed.png")

		"ice":
			$NameCostContainer/Name.text = "Special"
			$Container/StatNum.text = ""
			$Container/Stat.text = ""
			$Img.texture = preload("res://assets/abilities/ice.png")

			# specific type of special doesn't matter anymore
			# and this makes it easier to reference stuff in DinoInfo.gd
			button_mode = "special"

		"fire":
			$NameCostContainer/Name.text = "Special"
			$Container/StatNum.text = ""
			$Container/Stat.text = ""
			$Img.texture = preload("res://assets/abilities/fire.png")

			button_mode = "special"
	info_set = true

	$Container/StatNum.text = str(DinoInfo.get_upgrade_stat(ShopInfo.shop_dino, button_mode))

	var cost = DinoInfo.get_next_upgrade_cost(ShopInfo.shop_dino, button_mode)
	money_cost = cost[0]
	gene_cost = cost[1]

func color_squares(color = Color(1, 1, 1, 1)):
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
			squares_list[j].color = Color(0, 1, 0, 1)
		else:
			squares_list[j].color = color


func set_upgrade_squares():
	max_squares = DinoInfo.get_max_upgrade(ShopInfo.shop_dino, button_mode)

	# make the background squares
	for i in max_squares:
		var new_square = ColorRect.new()
		new_square.color = Color(0, 0, 0, 0.5)
		new_square.rect_min_size = Vector2(container_length / max_squares, container_height)
		new_square.rect_size = new_square.rect_min_size

		# don't make new squares every time
		if first_run:
			$Container/Squares.add_child(new_square)

	first_run = false

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
	# don't do anything if not enough money
	if ShopInfo.money < money_cost:
		return
	if ShopInfo.genes < gene_cost:
		return

	$Tween.interpolate_property($TextureProgress, "value", 0, 100, 1.5)
	$Tween.start()

func _on_UpgradeButton_button_up() -> void:
	stop_upgrading()


func _on_Tween_tween_completed(_object: Object, _key: NodePath) -> void:
	ShopInfo.money -= money_cost
	ShopInfo.genes -= gene_cost

	DinoInfo.add_upgrade(ShopInfo.shop_dino, button_mode)
	Signals.emit_signal("dino_upgraded")

	stop_upgrading()
	do_everything()
