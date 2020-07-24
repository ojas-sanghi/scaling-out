extends Area2D

var health := 150.0

onready var num_sprites = $Sprites.get_child_count()
onready var health_per_sprite = health / num_sprites

var current_sprite_num = 0
onready var current_sprite = $Sprites.get_children()[current_sprite_num]
onready var current_sprite_health = health_per_sprite

func _ready() -> void:
	Signals.connect("blockade_hit", self, "_on_blockade_hit")

func _on_blockade_hit(dmg: int):
	health -= dmg
	current_sprite_health -= dmg

	# set transparency to health / max health
	if current_sprite_health > 0:
		current_sprite.modulate = Color(1, 1, 1, current_sprite_health / health_per_sprite)
	# if destroyed, hide it and move to next sprite
	else:
		current_sprite.visible = false
		current_sprite_num += 1

		# only try to get sprite if it exists
		if current_sprite_num < num_sprites:
			current_sprite = $Sprites.get_children()[current_sprite_num]
			current_sprite_health = health_per_sprite
		# once all barriers destroyed, they won
		else:
			Signals.emit_signal("conquest_won")
			return


func _on_Blockade_area_entered(area: Area2D) -> void:
	if "Fire" in area.name:
		Signals.emit_signal("proj_hit", "fire")
	elif "Ice" in area.name:
		Signals.emit_signal("proj_hit", "ice")

