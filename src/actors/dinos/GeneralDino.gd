class_name GeneralDino, "res://assets/dinos/mega_dino/mega_dino.png"
extends Area2D

onready var bar = $HealthBar

var dino_dead := false

var dino_type: int
var dino_variation: int

var dino_health: int
var animated_health: int
var dino_speed: Vector2
var dino_dmg: int
var dino_dodge_chance: int
var dino_defense: float

var dino_unlock_cost: Array
var special_gene_type: String

var spawn_delay_value: int

var spawning_in := true

# this is set by the "Lane" node when the dino is instanced
var path_follow_time := 1
onready var path = $PathFollowTween

#note:
# for animation, we use AnimationPlayer for walk
# and AnimatedSprite for death

func spawn_delay():
	$HealthBar.hide()
	$AnimatedSprite.rotation_degrees = -90

	randomize()
	dino_variation = randi() % 3 + 1

	$AnimatedSprite.animation = str(dino_variation) + "walk"

	var tween = $TransparencyTween
	tween.interpolate_property(self, "modulate", Color(1, 1, 1, 0), Color(1, 1, 1, 1), spawn_delay_value)
	tween.start()
	yield(tween, "tween_completed")

	spawning_in = false

	Signals.emit_signal("dino_fully_spawned")

func _ready() -> void:
	# animate the spawn delay
	yield(spawn_delay(), "completed")

	bar.max_value = dino_health
	bar.value = dino_health
	$HealthBar.show()

	$ThumpSound.play()
	$AnimationPlayer.play(str(dino_variation) + "walk")

	path.interpolate_property(
		get_parent(), "unit_offset", 0, 1, path_follow_time, path.TRANS_LINEAR, path.EASE_IN_OUT
	)
	path.start()

	Signals.connect("dino_hit", self, "update_health")

func calculate_upgrades():
	var dino_info = DinoInfo.get_dino(dino_type)

	dino_health = dino_info.get_stat(Enums.stats.hp)
	animated_health = dino_health
	spawn_delay_value = dino_info.get_stat(Enums.stats.delay)

	dino_defense = dino_info.get_stat(Enums.stats.def)
	dino_dodge_chance = dino_info.get_stat(Enums.stats.dodge)
	dino_dmg = dino_info.get_stat(Enums.stats.dmg)
	dino_speed = Vector2(dino_info.get_stat(Enums.stats.speed), 0)

func _process(_delta: float) -> void:
	bar.value = animated_health


func kill_dino():
	remove_from_group("dinos")
	Signals.emit_signal("dino_died", dino_type)

	$CollisionPolygon2D.set_deferred("disabled", true)
	path.set_active(false)

	randomize()
	var num = randi() % 2
	if num == 0:
		$AnimatedSprite.flip_h = true
	else:
		$AnimatedSprite.flip_h = false

	$DeathSound.play()
	$AnimationPlayer.stop()
	$AnimatedSprite.play(str(dino_variation) + "death")

	var tween = $TransparencyTween
	tween.interpolate_property(self, "modulate", Color(1, 1, 1, 1), Color(1, 1, 1, 0), 5)
	tween.start()
	yield(tween, "tween_completed")
	yield($DeathSound, "finished")

	queue_free()

	CombatInfo.dinos_died += 1
	if CombatInfo.dinos_died == CombatInfo.max_dinos:
		Signals.emit_signal("conquest_lost")


func update_health(dmg_taken):
	dmg_taken *= dino_defense

	dino_health -= dmg_taken
	$HealthTween.interpolate_property(
		self,
		"animated_health",
		animated_health,
		dino_health,
		0.6,
		Tween.TRANS_LINEAR,
		Tween.EASE_IN
	)
	if not $HealthTween.is_active():
		$HealthTween.start()

	if dino_health <= 0:
		if not dino_dead:
			kill_dino()
			dino_dead = true

func attack_blockade():
	path.set_active(false)
	# play attacking animations
	$AnimationPlayer.stop()

	$AttackingTimer.start()

func _on_AttackingTimer_timeout() -> void:
	Signals.emit_signal("blockade_hit", dino_dmg)
	attack_blockade()

func _on_GeneralDino_area_entered(area: Area2D) -> void:
	if "Blockade" in area.name:
		attack_blockade()
