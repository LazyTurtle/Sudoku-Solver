extends OptionButton

var row : int
var column : int

signal item_selected_with_id(index, row_id, column_id)

func init(row_id : int, column_id : int):
	row = row_id
	column = column_id
	pass
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func _on_EditableCell_item_selected(index):
	emit_signal("item_selected_with_id",index, row, column)
