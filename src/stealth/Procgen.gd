extends Node2D

const N = 1
const E = 2
const S = 4
const W = 8

var cell_walls = {Vector2(0, -3): N, Vector2(3, 0): E,
				  Vector2(0, 3): S, Vector2(-3, 0): W}

var tile_size = 16  # tile size (in pixels)
var width = 1920 / tile_size  # width of map (in tiles)
var height = 1080 / tile_size  # height of map (in tiles)

var map_seed = 675343778

# get a reference to the map for convenience
onready var map = $Gray
onready var bg_map = $GrayBG

onready var cam = $Player/Camera2D

func _ready():
	randomize()
	if !map_seed:
		map_seed = randi()
	seed(map_seed)

	tile_size = map.cell_size
	make_maze()

func check_neighbors(cell, unvisited):
	# returns an array of cell's unvisited neighbors
	var list = []
	for n in cell_walls.keys():
		if cell + n in unvisited:
			list.append(cell + n)
	return list

func make_maze():
	var unvisited = []  # array of unvisited tiles
	var stack = []
	# fill the map with solid tiles
	map.clear()
	for x in range(width):
		for y in range(height):
			map.set_cell(x, y, 47) #0)
			bg_map.set_cell(x, y, 0)

	for x in range(0, width, 3):
		for y in range(0, height, 3):
			unvisited.append(Vector2(x, y))
	var current = Vector2(0, 0)
	unvisited.erase(current)

	# execute recursive backtracker algorithm
	while unvisited:
		var neighbors = check_neighbors(current, unvisited)
		if neighbors.size() > 0:
			var next = neighbors[randi() % neighbors.size()]
			stack.append(current)
			# remove walls from *both* cells
			var dir = next - current
			var current_walls = map.get_cellv(current) - cell_walls[dir]
			var next_walls = map.get_cellv(next) - cell_walls[-dir]
			map.set_cellv(current, current_walls)
			map.set_cellv(next, next_walls)

			# insert intermediate cell
			# horizontal
			if dir.x != 0:
				map.set_cellv(current + dir/3, 36)
			# vertical
			else:
				map.set_cellv(current + dir/3, 16)

			current = next
			unvisited.erase(current)
		elif stack:
			current = stack.pop_back()

#		yield(get_tree(), 'idle_frame')
