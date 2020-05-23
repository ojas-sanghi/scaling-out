tool
extends Node2D

export var sprite: Texture
export var text: String

export(String, "none", "fire", "ice") var ability_mode := "none"

export var custom_scale := Vector2(0.511, 0.519)

var id: int

func _ready() -> void:
	$Sprite.texture = sprite
	$Label.text = text
	id = int(text) - 1
	$Sprite.scale = custom_scale
	hide_particles()

	if ability_mode == "none":
		$disabled.hide()
	else:
		disable_ability()
		$DeployTimer.hide()

	if Engine.editor_hint:
		if not $Sprite.texture:
			$Sprite.texture = preload("res://assets/dinos/mega_dino/mega_dino.png")
			$Label.text = "1"
			id = 0
			$disabled.hide()
			hide_particles()

	DinoInfo.connect("dino_deployed", self, "_on_dino_deployed")


func fade_sprite():
	$Sprite.modulate = Color(1, 1, 1, 0.5)


func unfade_sprite():
	$Sprite.modulate = Color(1, 1, 1, 1)


func disable_ability():
	if ability_mode == "none":
		return

	$disabled.show()
	fade_sprite()


func enable_ability():
	if ability_mode == "none":
		return

	$disabled.hide()
	unfade_sprite()


func hide_particles():
	$Particles.hide()


func show_particles():
	$Particles.show()


func _on_dino_deployed():
	if DinoInfo.dino_id != id:
		return

	fade_sprite()
	yield(get_tree().create_timer(DinoInfo.get_dino_timer_delay(), false), "timeout")
	unfade_sprite()
