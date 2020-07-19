## Credit for this script and scene goes to John Gabriel
## Taken from his game, "Hijnx", made with Godot
## GPLv3 license
## https://bitbucket.org/JohnGabrielUK/hijinx/src/master/
#
extends Node2D
#
#const SHADOW_RESOLUTION = 360
#
#export (Color) var light_colour setget update_colour
#export (float) var light_size setget update_size
##export (bool) var flame_visible = false
#
#onready var raycast = $RayCast2D
##onready var flame = $Flame
#
#var ray_lengths
#
#func update_colour(value):
#	light_colour = value
#	update()
#
#func update_size(value):
#	light_size = value
#	update()
#
#func update_ray_lengths():
#	for i in range(0, SHADOW_RESOLUTION):
#		var angle = (float(i)/SHADOW_RESOLUTION)*PI*2.0
#		raycast.cast_to = make_point(angle, light_size)
#		raycast.force_raycast_update()
#		if raycast.is_colliding():
#			var collision_pos = raycast.get_collision_point() - global_position
#			var collision_length = Vector2().distance_to(collision_pos)
#			ray_lengths[i] = collision_length
#		else:
#			ray_lengths[i] = light_size
#	update()
#
#func make_point(direction, amount):
#	var result = Vector2(0.0, 0.0)
#	result.x += cos(direction) * amount
#	result.y -= sin(direction) * amount
#	return result
#
#func _draw():
#	#if Options.get_complex_lights() == false: return
#	#draw_circle(Vector2(0.0, 0.0), 5.0, Color.green)
#	var points = PoolVector2Array()
#	var colors = PoolColorArray()
#	for i in range(0, SHADOW_RESOLUTION):
#		var index_plus = i + 1
#		if index_plus == SHADOW_RESOLUTION: index_plus = 0
#		var angle_a = (float(i)/SHADOW_RESOLUTION)*PI*2.0
#		var angle_b = (float(i+1)/SHADOW_RESOLUTION)*PI*2.0
#		var power_a = ray_lengths[i]/light_size
#		var power_b = ray_lengths[index_plus]/light_size
#		points.append(Vector2(0.0, 0.0))
#		points.append(make_point(angle_a, ray_lengths[i]))
#		points.append(make_point(angle_b, ray_lengths[index_plus]))
#		colors.append(light_colour)
#		colors.append(light_colour.linear_interpolate(Color(0.0, 0.0, 0.0, 0.0), power_a))
#		colors.append(light_colour.linear_interpolate(Color(0.0, 0.0, 0.0, 0.0), power_b))
#	draw_polygon(points, colors)
#
#func _ready():
#	update_ray_lengths()
#	update()
#	#flame.visible = flame_visible
#
#func _enter_tree():
#	ray_lengths = []
#	for i in range(0, SHADOW_RESOLUTION):
#		ray_lengths.append(10.0)
