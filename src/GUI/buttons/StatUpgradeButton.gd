extends UpgradeButton


func set_button_info():
	text = ""

	match button_mode:
		"hp":
			$Container/StatNum.add_color_override("font_color", Color("e61a1a"))
			$Container/Stat.add_color_override("font_color", Color("e61a1a"))

			$Container/Stat.text = "HP"

			$Img.texture = preload("res://assets/icons/heart.png")

			button_mode = Enums.stats.hp

		"delay":
			$Container/StatNum.add_color_override("font_color", Color("1a82e6"))
			$Container/Stat.add_color_override("font_color", Color("1a82e6"))

			$Container/Stat.text = "s"

			$Img.texture = preload("res://assets/icons/timer.png")

			button_mode = Enums.stats.delay

	info_set = true
	var dino_info = DinoInfo.get_dino(ShopInfo.shop_dino)


	$Container/StatNum.text = str(dino_info.get_stat(button_mode))

	var cost = dino_info.get_next_upgrade_cost(button_mode)
	gold_cost = cost[0]
	gene_cost = cost[1]

