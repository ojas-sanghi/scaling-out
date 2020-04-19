extends Area2D

signal level_passed

func _on_Vault_body_entered(body: Node) -> void:
	if body.name == "Player":
		emit_signal("level_passed")
