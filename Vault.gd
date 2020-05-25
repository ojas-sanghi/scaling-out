extends Area2D

signal level_passed
export var ice := true

func _ready() -> void:
	if ice:
		$AnimatedSprite.animation = "ice"
	else:
		$AnimatedSprite.animation = "fire"

func _on_Vault_body_entered(body: Node) -> void:
	if body.name == "Player":
		var anim_name = $AnimatedSprite.animation
		if anim_name == "ice":
			ShopInfo.finding_ice = false
			DinoInfo.add_upgrade("tanky", "ice")
		else:
			ShopInfo.finding_fire = false
			DinoInfo.add_upgrade("warrior", "fire")
		emit_signal("level_passed")
