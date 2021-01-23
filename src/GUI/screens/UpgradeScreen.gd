extends Control


func _ready() -> void:
	set_info()


func set_info():
	match ShopInfo.shop_dino:
		Enums.dinos.tanky:
			$NameContainer/Name.text = "TANKY DINO"
			$Image.texture = preload("res://assets/dinos/tanky_dino/Armored_Dino_ICON.png")
		Enums.dinos.warrior:
			$NameContainer/Name.text = "WARRIOR DINO"
			$Image.texture = preload("res://assets/dinos/warrior_dino/Tribal_Dino_icon.png")
		Enums.dinos.mega:
			$NameContainer/Name.text = "MEGA DINO"
			$Image.texture = preload("res://assets/dinos/mega_dino/mega_dino.png")

	# TODO: add gator dino
