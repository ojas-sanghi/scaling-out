extends Control

var id := 0
onready var dino_timer = $Timer


func _ready() -> void:
	Signals.connect("dino_deployed", self, "_on_dino_deployed")

	# give the circle an id relating to each dino in the list
	for i in Enums.dinos.values():
		if i in CombatInfo.selector_timer_list:
			continue
		CombatInfo.selector_timer_list.append(i)
		id = i
		break

	$TextureProgress.hide()


func _on_dino_deployed():
	# only bother if the dino being deployed is our associated id
	if CombatInfo.dino_id != id:
		return

	var delay = DinoInfo.get_dino_timer_delay()
	dino_timer.start(delay)

	update_progress_bar()


func update_progress_bar():
	$TextureProgress.show()

	var delay = DinoInfo.get_dino_timer_delay()
	$Tween.interpolate_property($TextureProgress, "value", 0, 100, delay)
	$Tween.start()


func _on_Timer_timeout() -> void:
	$TextureProgress.hide()
