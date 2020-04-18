extends Area2D
class_name GeneralMonster

# Speed
export var speed := Vector2(50, 0)
var monster_health = 100
var animated_health = monster_health
var monster_dead = false

onready var bar = $HealthBar

var monster_color = "blue"

func _ready() -> void:
	bar.max_value = monster_health
	randomize()
	var color_num = randi() % 3
	match color_num:
		0:
			monster_color = "blue"
		1:
			monster_color = "green"
		2:
			monster_color = "orange"
	$AnimatedSprite.play(monster_color + "_walk")

	$ThumpSound.play()

	$AnimatedSprite.rotation_degrees = -90
	$CollisionShape2D.rotation_degrees = -90

func _physics_process(delta: float) -> void:
	 self.position += speed * delta

func _process(delta: float) -> void:
	var round_value = round(animated_health)
	bar.value = round_value

func game_over():
	get_tree().paused = true
	get_node("/root/CombatScreen/LoseDialogue").show()

func monster_died():
	$CollisionShape2D.set_deferred("disabled", true)
	set_physics_process(false)
#	$AnimatedSprite.rotation_degrees = 0
#	$CollisionShape2D.rotation_degrees = 0

	randomize()
	var num = randi() % 2
	$AnimatedSprite.play(monster_color + "_death" + str(num))
	var tween = $TransparencyTween
	tween.interpolate_property(self, "modulate", Color(1,1,1,1), Color(1,1,1,0), 1)
	$DeathSound.play()
	tween.start()
	yield(tween, "tween_completed")
	queue_free()

	Globals.monsters_died += 1
	if Globals.monsters_died == Globals.max_monsters:
		game_over()

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
	get_tree().paused = true
	get_node("/root/CombatScreen/WinDialogue").show()

func _on_GeneralMonster_area_entered(area: Area2D) -> void:
	if "Bullet" in area.name:
		area.queue_free()
		update_health()
	if "Blockade" in area.name:
		win_game()

func _on_ThumpSound_finished() -> void:
	$ThumpSound.stop()

func _on_DeathSound_finished() -> void:
	$DeathSound.stop()
