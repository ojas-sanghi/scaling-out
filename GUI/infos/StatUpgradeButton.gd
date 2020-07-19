extends UpgradeButton


func set_button_info():
	text = ""
	$Container/StatNum.text = str(DinoInfo.get_upgrade_stat(ShopInfo.shop_dino, button_mode))

	match button_mode:
		"hp":
			$Container/StatNum.add_color_override("font_color", Color("e61a1a"))
			$Container/Stat.add_color_override("font_color", Color("e61a1a"))

			$Container/Stat.text = "HP"

			$Img.texture = preload("res://assets/icons/heart.png")

		"delay":
			$Container/StatNum.add_color_override("font_color", Color("1a82e6"))
			$Container/Stat.add_color_override("font_color", Color("1a82e6"))

			$Container/Stat.text = "s"

			$Img.texture = preload("res://assets/icons/timer.png")
