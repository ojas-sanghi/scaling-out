extends Control

onready var dino_timer = $Timer
var id := 0

func _ready() -> void:
	var lanes = get_tree().get_nodes_in_group("lanes")
	if lanes:
		for lane in lanes:
			lane.connect("dino_deployed", self, "_on_dino_deployed")

	# give the circle an id relating to each dino in the list
	for i in range(0, DinoInfo.dino_list.size()):
		if i in DinoInfo.timer_list:
			continue
		DinoInfo.timer_list.append(i)
		id = i
		break

	$TextureProgress.hide()

func _on_dino_deployed():
	# only bother if the dino being deployed is our associated id
	if DinoInfo.dino_id != id:
		return

	# start timer delay
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
