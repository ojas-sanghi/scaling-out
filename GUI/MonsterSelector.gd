extends Node2D

export var id = 1
var active_id := 1

func _ready() -> void:
	$Particles.visible = false
	var str_id = str(id)
	$Label.text = str_id

func _process(delta: float) -> void:
	if id == active_id:
		$Particles.visible = true
	else:
		$Particles.visible = false

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("monster_1"):
		active_id = 1
	elif event.is_action_pressed("monster_2"):
		active_id = 2
	elif event.is_action_pressed("monster_3"):
		active_id = 3
	else:
		pass

func _on_MonsterSelector_pressed() -> void:
	active_id = id
	Globals.monster_id = active_id
