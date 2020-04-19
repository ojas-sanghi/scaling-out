extends Node2D

export var id = 1
var active_id := 1

func _ready() -> void:
	$Label1.text = "1"
	$Label2.text = "2"
	$Label3.text = "3"

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("monster_1"):
		active_id = 1
		$Particles.position.x = 0
	elif event.is_action_pressed("monster_2"):
		active_id = 2
		$Particles.position.x = 140
	elif event.is_action_pressed("monster_3"):
		active_id = 3
		$Particles.position.x = 270
	else:
		pass
	Globals.monster_id = active_id
