extends Area2D

export var ice := true

func _ready() -> void:
	if ice:
		$AnimatedSprite.animation = "ice"
	else:
		$AnimatedSprite.animation = "fire"

func _on_Vault_body_entered(body: Node) -> void:
	var anim_name = $AnimatedSprite.animation
	if anim_name == "ice":
		ShopInfo.finding_ice = false
		DinoInfo.add_upgrade("tanky", "ice")
	else:
		ShopInfo.finding_fire = false
		DinoInfo.add_upgrade("warrior", "fire")
	Signals.emit_signal("level_passed")
