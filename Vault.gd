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
			Globals.finding_ice = false
		else:
			Globals.finding_fire = false
		Globals.upgrades[anim_name] = true
		emit_signal("level_passed")

