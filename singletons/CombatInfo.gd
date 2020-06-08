extends Node

# holds conquest/round-specific info
# placed in a singleton for ease of access
# reset between conquests

var max_dinos: int
var dinos_remaining: int
var dinos_died: int

var dino_id: int

var dinos_deploying: Array
var selector_timer_list: Array

var shot_ice: bool
var shot_fire: bool

func _ready() -> void:
	reset()

	Signals.connect("dino_deployed", self, "_on_dino_deployed")

func reset(_max_dinos := 10) -> void:
	max_dinos = _max_dinos
	dinos_remaining = _max_dinos
	dinos_died = 0

	dino_id = 0

	dinos_deploying = []
	selector_timer_list = []

	shot_ice = false
	shot_fire = false

func _on_dino_deployed():
	# add to list of dinos just deployed
	# prevents this type of dino from being deployed
	dinos_deploying.append(dino_id)

	# wait for the dino-specific delay
	var delay = DinoInfo.get_dino_timer_delay()
	var dinos_deploying_timer = Timer.new()
	dinos_deploying_timer.one_shot = true
	add_child(dinos_deploying_timer)

	dinos_deploying_timer.connect("timeout", self, "_on_dinos_deploying_timer_timeout", [dino_id])
	dinos_deploying_timer.start(delay)

func _on_dinos_deploying_timer_timeout(id):
	# once the delay is over, let the dino be deployed again
	dinos_deploying.remove(dinos_deploying.find(id))
