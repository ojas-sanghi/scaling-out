extends Control

onready var dino_timer = $Timer
var counting_down = false
var id := 0

func _ready() -> void:
	var lanes = get_tree().get_nodes_in_group("lanes")
	if lanes:
		for lane in lanes:
			lane.connect("dino_deployed", self, "_on_dino_deployed")

	$TextureProgress.hide()

	# give them an id relating to each dino in the list
	for i in range(0, DinoInfo.dino_list.size()):
		if i in DinoInfo.timer_list:
			continue
		DinoInfo.timer_list.append(i)
		id = i
		break

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
	counting_down = true

	var delay = DinoInfo.get_dino_timer_delay()

	$TextureProgress.value = (dino_timer.time_left / delay) * 100

	# once we're done, set bool to false and hide the circle
	if dino_timer.time_left == 0:
		counting_down = false
		$TextureProgress.hide()

func _process(delta: float) -> void:
	# only update in _process if the delay is active
	if counting_down:
		update_progress_bar()
