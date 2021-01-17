extends Control


func _ready() -> void:
	set_info()


func set_info():
	match ShopInfo.shop_dino:
		"tanky":
			$NameContainer/Name.text = "TANKY DINO"
			$Image.texture = preload("res://assets/dinos/tanky_dino/Armored_Dino_ICON.png")
		"warrior":
			$NameContainer/Name.text = "WARRIOR DINO"
			$Image.texture = preload("res://assets/dinos/warrior_dino/Tribal_Dino_icon.png")
		"mega":
			$NameContainer/Name.text = "MEGA DINO"
			$Image.texture = preload("res://assets/dinos/mega_dino/mega_dino.png")
