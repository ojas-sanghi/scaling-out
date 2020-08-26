extends Control


func _ready() -> void:
	set_text()

	Signals.connect("round_won", self, "set_text")

func set_text():
	$Label.text = "Round: " + str(CombatInfo.current_round) + " / " + str(CombatInfo.max_rounds)

