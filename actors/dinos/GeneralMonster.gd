class_name GeneralMonster, "res://assets/dinos/mega_lizard/mega_lizard.png"
extends Area2D

signal game_over

onready var bar = $HealthBar

var monster_dead := false

var monster_variations: Array
var monster_color: String

var monster_health: int
var animated_health: int
var monster_speed: Vector2
var monster_dodge_chance: int
var monster_dmg: int

var deploy_delay: int

func _init(
		_speed = Vector2(50, 0),
		_health = 100,
		_variations = ["blue", "green", "orange"],
		_dmg = 5,
		_delay = 2,
		_dodge = 0
	) -> void:
	monster_speed = _speed

	monster_health = _health
	animated_health = _health

	monster_variations = _variations
	monster_color = monster_variations[0]

	monster_dmg = _dmg
	monster_dodge_chance = _dodge

	deploy_delay = _delay

func _ready() -> void:
	$ThumpSound.play()
	bar.max_value = monster_health

	randomize()
	var color_num = randi() % 3
	monster_color = monster_variations[color_num]

	$AnimatedSprite.play(monster_color + "_walk")

	$AnimatedSprite.rotation_degrees = -90
	$CollisionShape2D.rotation_degrees = -90

	var army = get_tree().get_nodes_in_group("army")
	if army:
		for a in army:
			self.connect("game_over", a, "game_over")

func _physics_process(delta: float) -> void:
	 self.position += monster_speed * delta

func _process(delta: float) -> void:
	var round_value = round(animated_health)
	bar.value = round_value

func monster_died():
	$CollisionShape2D.set_deferred("disabled", true)
	set_physics_process(false)

	randomize()
	var num = randi() % 2
	$DeathSound.play()
	$AnimatedSprite.play(monster_color + "_death" + str(num))
	var tween = $TransparencyTween
	tween.interpolate_property(self, "modulate", Color(1,1,1,1), Color(1,1,1,0), 5)
	tween.start()
	yield(tween, "tween_completed")
	yield($DeathSound, "finished")

	queue_free()

	CombatInfo.dinos_died += 1
	if CombatInfo.dinos_died == CombatInfo.max_dinos:
		emit_signal("game_over")

func update_health():
	monster_health -= 17
	bar.value = monster_health
	$Tween.interpolate_property(self, "animated_health", animated_health, monster_health, 0.6, Tween.TRANS_LINEAR, Tween.EASE_IN)
	if not $Tween.is_active():
		$Tween.start()

	if monster_health <= 0:
		if not monster_dead:
			monster_died()
			monster_dead = true

func win_game():
	SceneChanger.go_to_scene("res://GUI/CombatWinDialogue.tscn")

func _on_GeneralMonster_area_entered(area: Area2D) -> void:
	if "Bullet" in area.name:
		area.queue_free()
		update_health()
	if "Blockade" in area.name:
		win_game()
