extends KinematicBody2D

export var speed = 300

var velocity
var player_is_immune = false
var player_powerups = []

var direction_from_player = Vector2()
var coins_in_level = 0

var guards

var node_path


func _ready() -> void:
	Signals.connect("level_failed", self, "_on_level_failed", [], CONNECT_ONESHOT)
	var coins = get_tree().get_nodes_in_group("coins")
	Signals.connect("coin_grabbed", self, "_on_coin_grabbed")
	Signals.connect("level_passed", self, "_on_level_passed")

	if StealthInfo.finding_ice:
		node_path = "/root/StealthIce/"
	else:
		node_path = "/root/StealthFire/"


func get_input():
	# Detect up/down/left/right keystate and only move when pressed
	velocity = Vector2(0, 0)
	if Input.is_action_pressed('ui_right'):
		velocity.x += 1
	if Input.is_action_pressed('ui_left'):
		velocity.x -= 1
	if Input.is_action_pressed('ui_down'):
		velocity.y += 1
	if Input.is_action_pressed('ui_up'):
		velocity.y -= 1

	return velocity


func animate_player():
	var velocity_length = velocity.length()
	var velocity_angle = velocity.angle()
	# Convert it to degrees
	velocity_angle = rad2deg(velocity_angle)
	# Add 90 degrees since otherwise it treats going right as 0 degrees
	velocity_angle = velocity_angle + 90

	# If we're moving, change rotation
	if velocity_length >= 1:
#		$AnimatedSprite.rotation_degrees = velocity_angle
#		$Collision.rotation_degrees = velocity_angle
		rotation_degrees = velocity_angle - 180

	if velocity_length >= 1:
		# If moving in any direction, play walk animation.
		$AnimatedSprite.play("walk")
	else:
		# Not moving, idle anim
		$AnimatedSprite.play("idle")


func _physics_process(delta):
	# Disable any movement if the player died
	if ShopInfo.player_caught:
		return

	velocity = get_input()
	velocity = velocity.normalized() * speed
	animate_player()
	move_and_slide(velocity)


func _on_level_passed():
	set_physics_process(false)
	$AnimatedSprite.stop()
	$Win.play()

	SceneChanger.fade()
	get_node(node_path + "CanvasModulate").hide()
	get_node(node_path + "CanvasLayer/CoinCounter").hide()
	yield($Win, "finished")
	get_node(node_path + "Vault").hide()
	SceneChanger.go_to_scene("res://src/GUI/dialogues/StealthWinDialogue.tscn")
	ShopInfo.gold += coins_in_level


func _on_level_failed():
#	set_physics_process(false)
#	$AnimatedSprite.stop()
#	$Caught.play()
#
#	SceneChanger.fade()
#	get_node(node_path + "CanvasModulate").hide()
#	get_node(node_path + "CanvasLayer/CoinCounter").hide()
#	yield($Caught, "finished")
#	get_node(node_path + "Vault").hide()
#	SceneChanger.go_to_scene("res://src/GUI/dialogues/StealthLoseDialogue.tscn")

	print("YOU FAILED")


func _on_coin_grabbed(value):
	coins_in_level += value
