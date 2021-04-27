extends Area2D

func _ready() -> void:
	if StealthInfo.finding_ice:
		$AnimatedSprite.animation = "ice"
	else:
		$AnimatedSprite.animation = "fire"


func _on_Vault_body_entered(body: Node) -> void:
	if StealthInfo.finding_ice:
		StealthInfo.finding_ice = false
		var tanky = DinoInfo.get_dino(Enums.dinos.tanky)
		tanky.upgrade(Enums.stats.special)
	else:
		StealthInfo.finding_fire = false
		var warrior = DinoInfo.get_dino(Enums.dinos.warrior)
		warrior.upgrade(Enums.stats.special)

	Signals.emit_signal("level_passed")
