extends GridContainer

class_name SudokuGrid

signal cell_value_changed(index,row,column)

export(PackedScene) var editable_cell = preload("res://Scenes/2D Nodes/EditableCell.tscn")
var cell_matrix : Array

func _ready():
	_fill_sudoku_grid()
	pass

func _create_cell_grid():
	var matrix = []
	for _i in range(9):
		matrix.append([])
	return matrix

func _fill_sudoku_grid():
	cell_matrix = _create_cell_grid()
	
	for i in range(9):
		for j in range(9):
			var cell = editable_cell.instance()
			cell.init(i,j)
			cell.connect("item_selected_with_id",self,"On_Cell_Value_Changed")
			cell_matrix[i].append(cell)
			self.add_child(cell)
	pass

func export_grid():
	return cell_matrix

func export_grid_into_matrix():
	var matrix = []
	for i in range(9):
		matrix.append([])
		for j in range(9):
			matrix[i].append(cell_matrix[i][j].get_item_id())
	return matrix

func On_Cell_Value_Changed(index: int, row: int, column:int):
	emit_signal("cell_value_changed",index,row,column)
