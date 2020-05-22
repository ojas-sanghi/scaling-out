extends Control

onready var dino_timer = $Timer
var counting_down = false
var id := 0

func _ready() -> void:
	var lanes = get_tree().get_nodes_in_group("lanes")
	if lanes:
		for lane in lanes:
			lane.connect("monster_deployed", self, "_on_monster_deployed")

	$TextureProgress.hide()

	for i in range(0, DinoInfo.monster_list.size()):
		if i in DinoInfo.timer_list:
			continue
		DinoInfo.timer_list.append(i)
		id = i
		break

func _on_monster_deployed():
	if counting_down:
		return
	if DinoInfo.monster_id != id:
		return

	var delay = DinoInfo.get_dino_timer_delay()
	dino_timer.start(delay)

	update_progress_bar()

func update_progress_bar():
	$TextureProgress.show()
	counting_down = true

	var delay = DinoInfo.get_dino_timer_delay()

	$TextureProgress.value = (dino_timer.time_left / delay) * 100

	if dino_timer.time_left == 0:
		counting_down = false
		$TextureProgress.hide()

func _process(delta: float) -> void:
	if counting_down:
		update_progress_bar()
