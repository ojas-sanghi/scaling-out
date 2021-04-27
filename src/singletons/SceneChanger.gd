extends Control


func fade():
	var anim_player = $CanvasLayer/AnimationPlayer
	anim_player.play("fade")
	yield(anim_player, "animation_finished")
	$CanvasLayer/ColorRect.hide()


func go_to_scene(scene_path):
	yield(fade(), "completed")

	get_tree().change_scene(scene_path)
