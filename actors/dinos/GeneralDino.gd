class_name GeneralDino, "res://assets/dinos/mega_dino/mega_dino.png"
extends Area2D

onready var bar = $HealthBar

var dino_dead := false

var dinosaur_name: String

var dino_variations: Array
var dino_color: String

var dino_health: int
var animated_health: int
var dino_speed: Vector2
var dino_dmg: int
var dino_dodge_chance: int

var dino_gene_cost: int
var special_gene_type: String

var deploy_delay: int
var spawn_delay: int

var spawning_in := true

func _init(
		_name = "",
		_speed = Vector2(50, 0),
		_health = 100,
		_variations = ["blue", "green", "orange"],
		_dmg = 5,
		_cost = 100,
		_gene = "",
		_ddelay = 2,
		_sdelay = 1.5,
		_dodge = 0
	) -> void:
	dinosaur_name = _name
	dino_speed = _speed

	dino_health = _health
	animated_health = _health

	dino_variations = _variations
	dino_color = dino_variations[0]

	dino_dmg = _dmg
	dino_dodge_chance = _dodge

	dino_gene_cost = _cost
	special_gene_type = _gene

	deploy_delay = _ddelay
	spawn_delay = _sdelay

func spawn_delay():
	set_physics_process(false)
	set_process(false)

	$HealthBar.hide()

	randomize()
	var color_num = randi() % 3
	dino_color = dino_variations[color_num]

	$AnimatedSprite.animation = (dino_color + "_walk")
	$AnimatedSprite.frame = 0
	$AnimatedSprite.stop()

	var tween = $TransparencyTween
	tween.interpolate_property(self, "modulate", Color(1,1,1,0), Color(1,1,1,1), 2)
	tween.start()
	yield(tween, "tween_completed")

	$AnimatedSprite.play()
	$HealthBar.show()
	spawning_in = false
	Signals.emit_signal("dino_fully_spawned")
	set_physics_process(true)
	set_process(true)

func _ready() -> void:
	# animate the spawn delay
	yield(spawn_delay(), "completed")


	$ThumpSound.play()
	bar.max_value = dino_health

	$AnimatedSprite.play(dino_color + "_walk")

	$AnimatedSprite.rotation_degrees = -90
	$CollisionShape2D.rotation_degrees = -90


func _physics_process(delta: float) -> void:
	 self.position += dino_speed * delta

func _process(delta: float) -> void:
	var round_value = round(animated_health)
	bar.value = round_value

func kill_dino():
	remove_from_group("dinos")
	Signals.emit_signal("dino_died", dinosaur_name)

	$CollisionShape2D.set_deferred("disabled", true)
	set_physics_process(false)

	randomize()
	var num = randi() % 2
	$DeathSound.play()
	$AnimatedSprite.play(dino_color + "_death" + str(num))
	var tween = $TransparencyTween
	tween.interpolate_property(self, "modulate", Color(1,1,1,1), Color(1,1,1,0), 5)
	tween.start()
	yield(tween, "tween_completed")
	yield($DeathSound, "finished")

	queue_free()

	CombatInfo.dinos_died += 1

	if CombatInfo.dinos_died == CombatInfo.max_dinos:
		Signals.emit_signal("game_over")

func update_health():
	dino_health -= 17
	bar.value = dino_health
	$Tween.interpolate_property(self, "animated_health", animated_health, dino_health, 0.6, Tween.TRANS_LINEAR, Tween.EASE_IN)
	if not $Tween.is_active():
		$Tween.start()

	if dino_health <= 0:
		if not dino_dead:
			kill_dino()
			dino_dead = true

func win_game():
	SceneChanger.go_to_scene("res://GUI/CombatWinDialogue.tscn")

func _on_GeneralDino_area_entered(area: Area2D) -> void:
	if "Bullet" in area.name:
		area.queue_free()
		update_health()
	if "Blockade" in area.name:
		win_game()
