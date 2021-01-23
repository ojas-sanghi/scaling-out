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
		var tanky = DinoInfo.get_dino(Enums.dinos.tanky)
		tanky.upgrade(Enums.stats.special)
	else:
		ShopInfo.finding_fire = false

		var warrior = DinoInfo.get_dino(Enums.dinos.warrior)
		warrior.upgrade(Enums.stats.special)
	Signals.emit_signal("level_passed")
