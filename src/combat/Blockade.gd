extends Area2D

var health := 150.0

var num_sprites
var health_per_sprite

var current_sprite_num
var current_sprite
var current_sprite_health

func _ready() -> void:
	Signals.connect("blockade_hit", self, "_on_blockade_hit")
	Signals.connect("new_round", self, "_on_new_round")

	reset()

func reset():
	health = 150
	num_sprites = $Sprites.get_child_count()
	health_per_sprite = health / num_sprites

	current_sprite_num = 0
	current_sprite = $Sprites.get_children()[current_sprite_num]
	current_sprite_health = health_per_sprite

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
		else:
			Signals.emit_signal("round_won")
			return

func _on_new_round():
	for sprite in $Sprites.get_children():
		sprite.modulate = Color(1, 1, 1, 1)
		sprite.visible = true
	reset()
