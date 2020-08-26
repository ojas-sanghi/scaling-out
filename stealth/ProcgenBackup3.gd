extends Node2D

const N = 1
const E = 2
const S = 4
const W = 8

var cell_walls = {Vector2(0, -2): N, Vector2(2, 0): E,
				  Vector2(0, 2): S, Vector2(-2, 0): W}

var tile_size = 16  # tile size (in pixels)
var width = 120  # width of map (in tiles)
var height = 67.5  # height of map (in tiles)

var map_seed = 675343778

# fraction of walls to remove
var erase_fraction = 0.2

# get a reference to the map for convenience
onready var Map = $Gray
onready var cam = $Player/Camera2D

func _ready():
	cam.zoom = Vector2(3, 3)
	cam.position = Map.map_to_world(Vector2(width/2, height/2))

	randomize()
	if !map_seed:
		map_seed = randi()
	seed(map_seed)

	tile_size = Map.cell_size

	make_maze()
	erase_walls()
#	Map.update_bitmask_region(Vector2(0, 0), Vector2(1920, 1080))

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
	Map.clear()
	for x in range(width):
		for y in range(height):
			Map.set_cellv(Vector2(x, y), 0)
	for x in range(0, width, 2):
		for y in range(0, height, 2):
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
			var current_walls = Map.get_cellv(current) - cell_walls[dir]
			var next_walls = Map.get_cellv(next) - cell_walls[-dir]
			Map.set_cellv(current, current_walls)
			Map.set_cellv(next, next_walls)
			# insert intermediate cell
			if dir.x != 0:
				Map.set_cellv(current + dir/2, 5)
			else:
				Map.set_cellv(current + dir/2, 10)
			current = next
			unvisited.erase(current)
		elif stack:
			current = stack.pop_back()
#		yield(get_tree(), 'idle_frame')

func erase_walls():
	# randomly remove a number of the map's walls
	for i in range(int(width * height * erase_fraction)):
		var x = int(rand_range(2, width/2 - 2)) * 2
		var y = int(rand_range(2, height/2 - 2)) * 2
		var cell = Vector2(x, y)
		# pick random neighbor
		var neighbor = cell_walls.keys()[randi() % cell_walls.size()]
		# if there's a wall between them, remove it
		if Map.get_cellv(cell) & cell_walls[neighbor]:
			var walls = Map.get_cellv(cell) - cell_walls[neighbor]
			var n_walls = Map.get_cellv(cell+neighbor) - cell_walls[-neighbor]
			Map.set_cellv(cell, walls)
			Map.set_cellv(cell+neighbor, n_walls)
			# insert intermediate cell
			if neighbor.x != 0:
				Map.set_cellv(cell+neighbor/2, 5)
			else:
				Map.set_cellv(cell+neighbor/2, 10)
#		yield(get_tree(), 'idle_frame')

	for x in range(width):
		for y in range(height):
			var positions_around_tiles = [Map.get_cell(x - 1, y), Map.get_cell(x + 1, y), Map.get_cell(x, y - 1), Map.get_cell(x, y + 1), Map.get_cell(x - 1, y - 1), Map.get_cell(x + 1, y - 1), Map.get_cell(x + 1, y + 1), Map.get_cell(x - 1, y + 1)]

			var list_of_trues = []

			for each_pos in positions_around_tiles:
				if each_pos != 0:
					list_of_trues.append(true)
				else:
					list_of_trues.append(false)


			if false in list_of_trues:
				continue
			else:
				Map.set_cellv(Vector2(x, y), -1)
