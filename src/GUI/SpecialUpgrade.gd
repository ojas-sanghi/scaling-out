extends Control

export (String, "ice", "fire") var mode = "ice"


func _ready() -> void:
	set_text()


func set_text():
	match ShopInfo.shop_dino:
		Enums.dinos.tanky:
			$name.text = "Ice Projectile"
			$Sprite.texture = preload("res://assets/dinos/misc/ice.png")
			$VideoPlayer.stream = preload("res://assets/abilities/previews/ice-preview.ogv")
			$Description.text = "Unlock an ice projectile. When activated, will launch from a random tanky dinosaur on the map. On impact, slows down the rate of fire of the army. Can be used once per round."
		Enums.dinos.warrior:
			$name.text = "Fire Projectile"
			$Sprite.texture = preload("res://assets/dinos/misc/fire.png")
#			$VideoPlayer.stream = preload("res://assets/abilities/previews/ice-preview.ogv")
			$Description.text = "Unlock a fire projectile. When activated, will launch from a random warrior dino on the map. On impact, stops the army from firing from a few seconds. Can be used once per round."
	$VideoPlayer.play()

	# todo add gator?


func _on_VideoPlayer_finished() -> void:
	$VideoPlayer.play()
